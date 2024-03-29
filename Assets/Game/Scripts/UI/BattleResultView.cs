using System;
using CCore.Senary.Gameplay.Attacking;
using CCore.Senary.Input;
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

            GameStateMachine.Instance.GetState<AttackerWinBattleState>().ExitEvent += OnAttackerWinBattleStateExit;

            GameStateMachine.Instance.GetState<DefenderWinBattleState>().ExitEvent += OnDefenderWintBattleStateExit;

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

            EnableInputListener();
        }

        private void OnDefenderWintBattleStateEnter()
        {
            resultText.text = String.Format(
                originalResultString,
                "DEFENDER"
            );

            ShowSpecificResults();

            EnableInputListener();
        }

        private void OnAttackerWinBattleStateExit()
        {
            DisableInputListener();

            Hide();
        }

        private void OnDefenderWintBattleStateExit()
        {
            DisableInputListener();

            Hide();
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

        private void EnableInputListener()
        {
            PlayerInput.Instance.TapEvent += OnTap;
        }

        private void DisableInputListener()
        {
            PlayerInput.Instance.TapEvent -= OnTap;
        }

        private void OnTap(Vector2 position)
        {
            if (GameStateMachine.Instance.CurrentState is AttackerWinBattleState)
            {
                GameStateMachine.Instance.DoTransition<CheckForWinLoseTransition>();
            }
            else
            {
                GameStateMachine.Instance.DoTransition<AttackTransition>();
            }
        }
    }
}