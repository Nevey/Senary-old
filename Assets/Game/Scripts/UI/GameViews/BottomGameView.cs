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
    public class BottomGameView : UIView
    {
        [SerializeField] private Button endTurnButton;
        
        protected override void Setup()
        {
            GameStateMachine.Instance.GetState<AttackState>().EnterEvent += OnAttackStateEnter;

            GameStateMachine.Instance.GetState<AttackState>().ExitEvent += OnAttackStateExit;

            GameStateMachine.Instance.GetState<InvasionState>().ExitEvent += OnInvasionStateExit;

            InvasionController.Instance.AllowedToEndInvasionEvent += OnAllowedToEndInvasion;
        }

        private void OnAttackStateEnter()
        {
            EnableButtonListener();
        }

        private void OnAttackStateExit()
        {
            DisableButtonListener();
        }

        private void OnInvasionStateExit()
        {
            DisableButtonListener();
        }

        private void OnAllowedToEndInvasion()
        {
            EnableButtonListener();
        }

        private void EnableButtonListener()
        {
            endTurnButton.onClick.AddListener(EndTurnButtonClicked);
        }

        private void DisableButtonListener()
        {
            endTurnButton.onClick.RemoveListener(EndTurnButtonClicked);
        }

        private void EndTurnButtonClicked()
        {
            GameStateMachine.Instance.DoTransition<CheckForWinLostTransition>();
        }
    }
}