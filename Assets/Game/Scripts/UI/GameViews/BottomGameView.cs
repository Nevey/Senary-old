using System;
using CCore.Senary.StateMachines.Game;
using CCore.Senary.StateMachines.UI;
using CCore.UI;
using UnityEngine;
using UnityEngine.UI;

namespace CCore.Senary.UI
{
    public class BottomGameView : UIView
    {
        [SerializeField] private Button throwButton;
        
        protected override void Setup()
        {
            // GameStateMachine.Instance.GetState<ReceiveUnitsState>().EnterEvent += OnPlayerInputStateEnter;

            // GameStateMachine.Instance.GetState<ReceiveUnitsState>().ExitEvent += OnPlayerInputStateExit;
        }

        private void OnPlayerInputStateEnter()
        {
            throwButton.onClick.AddListener(OnThrowClicked);
        }

        private void OnPlayerInputStateExit()
        {
            throwButton.onClick.RemoveListener(OnThrowClicked);
        }

        private void OnThrowClicked()
        {
            GameStateMachine.Instance.DoTransition<ThrowDiceTransition>();
        }
    }
}