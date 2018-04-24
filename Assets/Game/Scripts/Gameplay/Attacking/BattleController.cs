using CCore.Senary.Gameplay.Dice;
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

        public BattleResult AttackerResult { get; private set; }

        public BattleResult DefenderResult { get; private set; }

        private void Awake()
        {
            GameStateMachine.Instance.GetState<BattleState>().EnterEvent += OnBattleStateEnter;
        }

        private void OnDestroy()
        {
            GameStateMachine.Instance.GetState<BattleState>().EnterEvent -= OnBattleStateEnter;
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

                    AttackerResult = new BattleResult(
                        e.throwResult,
                        AttackController.Instance.AttackingTile.UnitCount
                    );

                    break;
                
                case DieType.Defender:

                    DefenderResult = new BattleResult(
                        e.throwResult,
                        AttackController.Instance.DefendingTile.UnitCount
                    );
                    
                    break;
            }

            receivedResultCount++;

            if (receivedResultCount != throwResults.Length)
            {
                return;
            }
            
            receivedResultCount = 0;

            Compare();
        }

        private void Compare()
        {
            Destroy(throwDice);

            AttackController.Instance.ResetTileStates();
            
            // Attacker only wins if throw is higher then defender throw
            if (AttackerResult.FinalResult > DefenderResult.FinalResult)
            {
                Log("Attacker wins with {0} vs {1}", AttackerResult.FinalResult, DefenderResult.FinalResult);

                AttackerWins();
            }
            else
            {
                Log("Defender wins with {0} vs {1}", DefenderResult.FinalResult, AttackerResult.FinalResult);

                DefenderWins();
            }
        }

        private void AttackerWins()
        {
            Tile defendingTile = AttackController.Instance.DefendingTile;

            defendingTile.ClearUnits();
            defendingTile.ClearOwner();

            GameStateMachine.Instance.DoTransition<AttackerWinBattleTransition>();
        }

        private void DefenderWins()
        {
            Tile attackingTile = AttackController.Instance.AttackingTile;

            int unitsToRemove = attackingTile.UnitCount - 1;

            attackingTile.AddUnits(-unitsToRemove, attackingTile.Owner);

            GameStateMachine.Instance.DoTransition<DefenderWinBattleTransition>();
        }
    }
}