using System;
using System.Collections.Generic;
using CCore.Senary.Gameplay.Dice;
using CCore.Senary.Scenes;
using CCore.Senary.StateMachines.Game;
using CCore.Senary.Tiles;
using UnityEngine;

namespace CCore.Senary.Gameplay.Attacking
{
    public class BattleController : MonoBehaviourSingleton<BattleController>
    {
        [SerializeField] private GameObject throwDicePrefab;

        private GameObject throwDice;

        private ThrowResult[] throwResults;

        private int receivedResultCount = 0;

        private BattleResult attackerResult;

        private BattleResult defenderResult;

        public BattleResult AttackerResult { get { return attackerResult; } }

        public BattleResult DefenderResult { get { return defenderResult; } }

        private void Awake()
        {
            GameStateMachine.Instance.GetState<BattleState>().EnterEvent += OnBattleStateEnter;
        }

        private void OnBattleStateEnter()
        {
            throwDice = Instantiate(throwDicePrefab, Vector3.zero, Quaternion.identity, transform);

            throwResults = throwDice.GetComponentsInChildren<ThrowResult>();

            for (int i = 0; i < throwResults.Length; i++)
            {
                throwResults[i].ThrowResultEvent += OnThrowResult;
            }
        }

        private void OnThrowResult(object sender, ThrowResultArgs e)
        {
            switch (e.dieType)
            {
                case DieType.Attacker:

                    attackerResult = new BattleResult(
                        e.throwResult,
                        AttackController.Instance.AttackingTile.UnitCount
                    );

                    break;
                
                case DieType.Defender:

                    defenderResult = new BattleResult(
                        e.throwResult,
                        AttackController.Instance.DefendingTile.UnitCount
                    );
                    
                    break;
            }

            receivedResultCount++;

            if (receivedResultCount == throwResults.Length)
            {
                receivedResultCount = 0;

                Compare();
            }
        }

        private void Compare()
        {
            Destroy(throwDice);
            
            // Attacker only wins if throw is higher then defender throw
            if (attackerResult.FinalResult > defenderResult.FinalResult)
            {
                Log("Attacker wins with {0} vs {1}", attackerResult.FinalResult, defenderResult.FinalResult);

                AttackerWins();
            }
            else
            {
                Log("Defender wins with {0} vs {1}", defenderResult.FinalResult, attackerResult.FinalResult);

                DefenderWins();
            }
        }

        private void AttackerWins()
        {
            Tile defendingTile = AttackController.Instance.DefendingTile;

            defendingTile.ClearUnits();
            defendingTile.ClearOwner();

            GameStateMachine.Instance.DoTransition<AttackerWinBattleTransition>();

            // Go to attacker wins state
            // Go to invasion state
            // When done, go back to attack state
        }

        private void DefenderWins()
        {
            Tile attackingTile = AttackController.Instance.AttackingTile;

            int unitsToRemove = attackingTile.UnitCount - 1;

            attackingTile.AddUnits(-unitsToRemove, attackingTile.Owner);

            GameStateMachine.Instance.DoTransition<DefenderWinBattleTransition>();
            
            // Go to defender wins state
            // Go back to attack state
        }
    }
}