using System;
using CCore.Senary.Gameplay.Attacking;
using CCore.Senary.StateMachines.Game;
using CCore.Senary.StateMachines.UI;
using CCore.Senary.Tiles;
using CCore.UI;
using UnityEngine;
using UnityEngine.UI;

namespace CCore.Senary.UI
{
    public class EndTurnView : UIView
    {
        [SerializeField] private Button endTurnButton;
        
        protected override void Setup()
        {
            GameStateMachine.Instance.GetState<AttackState>().EnterEvent += OnAttackStateEnter;

            GameStateMachine.Instance.GetState<AttackState>().ExitEvent += OnAttackStateExit;
        }

        private void OnAttackStateEnter()
        {
            Show();

            endTurnButton.onClick.AddListener(EndTurnButtonClicked);
        }

        private void OnAttackStateExit()
        {
            Hide();
            
            endTurnButton.onClick.RemoveListener(EndTurnButtonClicked);
        }

        private void EndTurnButtonClicked()
        {
            GameStateMachine.Instance.DoTransition<CheckForWinLostTransition>();
        }
    }
}