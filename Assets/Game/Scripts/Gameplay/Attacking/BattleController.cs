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

        private List<ThrowResultArgs> receivedResults = new List<ThrowResultArgs>();

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
                    e.throwResult *= AttackController.Instance.AttackingTile.UnitCount;
                    break;
                
                case DieType.Defender:
                    e.throwResult *= AttackController.Instance.AttackingTile.UnitCount;
                    break;
            }

            receivedResults.Add(e);

            if (receivedResults.Count == throwResults.Length)
            {
                Compare();
            }
        }

        private void Compare()
        {
            int attackingThrowResult = 0;
            int defendingThrowResult = 0;

            for (int i = 0; i < receivedResults.Count; i++)
            {
                ThrowResultArgs result = receivedResults[i];

                // Safe since we only have 2 results at all times...
                switch (result.dieType)
                {
                    case DieType.Attacker:
                        attackingThrowResult = result.throwResult;
                        break;
                    
                    case DieType.Defender:
                        defendingThrowResult = result.throwResult;
                        break;

                    default:

                        // In case weird stuff happens, just not let the game break
                        attackingThrowResult = result.throwResult;
                        defendingThrowResult = result.throwResult;

                        LogWarning("No proper Die Type was assigned!");

                        break;
                }
            }

            // Attacker only wins if throw is higher then defender throw
            if (attackingThrowResult > defendingThrowResult)
            {
                Log("Attacker wins with {0} vs {1}", attackingThrowResult, defendingThrowResult);

                AttackerWins();
            }
            else
            {
                Log("Defender wins with {0} vs {1}", defendingThrowResult, attackingThrowResult);

                DefenderWins();
            }
        }

        private void AttackerWins()
        {
            Tile defendingTile = AttackController.Instance.DefendingTile;

            defendingTile.ClearUnits();
            defendingTile.ClearOwner();

            // Go to attacker wins state
            // Go to invasion state
            // When done, go back to attack state
        }

        private void DefenderWins()
        {
            Tile attackingTile = AttackController.Instance.AttackingTile;

            int unitsToRemove = attackingTile.UnitCount - 1;

            attackingTile.AddUnits(-unitsToRemove, attackingTile.Owner);

            // Go to defender wins state
            // Go back to attack state
        }
    }
}