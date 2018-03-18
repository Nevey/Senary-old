using System;
using CCore.Senary.Gameplay.Attacking;
using CCore.Senary.StateMachines.Game;
using CCore.UI;
using UnityEngine;
using UnityEngine.UI;

namespace CCore.Senary.UI
{
    public class BattleResultView : UIView
    {
        [SerializeField] private Text resultText;

        [SerializeField] private Text attackerText;

        [SerializeField] private Text defenderText;

        private string originalResultString;

        private string originalAttackerString;

        private string originalDefenderString;

        protected override void Setup()
        {
            GameStateMachine.Instance.GetState<AttackerWinBattleState>().EnterEvent += OnAttackerWinBattleStateEnter;

            GameStateMachine.Instance.GetState<DefenderWinBattleState>().EnterEvent += OnDefenderWintBattleStateEnter;

            originalResultString = resultText.text;

            originalAttackerString = attackerText.text;

            originalDefenderString = defenderText.text;
        }

        private void OnAttackerWinBattleStateEnter()
        {
            resultText.text = String.Format(
                originalResultString,
                "ATTACKER"
            );

            ShowSpecificResults();
        }

        private void OnDefenderWintBattleStateEnter()
        {
            resultText.text = String.Format(
                originalResultString,
                "DEFENDER"
            );

            ShowSpecificResults();
        }

        private void ShowSpecificResults()
        {
            Show();

            BattleResult attackerResult = BattleController.Instance.AttackerResult;

            attackerText.text = String.Format(
                originalAttackerString,
                attackerResult.ThrowResult,
                attackerResult.UnitCount,
                attackerResult.FinalResult
            );

            BattleResult defenderResult = BattleController.Instance.DefenderResult;

            defenderText.text = String.Format(
                originalDefenderString,
                defenderResult.ThrowResult,
                defenderResult.UnitCount,
                defenderResult.FinalResult
            );
        }
    }
}