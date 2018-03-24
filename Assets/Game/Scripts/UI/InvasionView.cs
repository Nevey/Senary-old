using System;
using CCore.Senary.Gameplay.Attacking;
using CCore.Senary.StateMachines.Game;
using CCore.UI;
using UnityEngine;
using UnityEngine.UI;

namespace CCore.Senary.UI
{
    public class InvasionView : UIView
    {
        [SerializeField] private Button endInvasionButton;
        
        protected override void Setup()
        {
            InvasionController.Instance.AllowedToEndInvasionEvent += OnAllowedToEndInvasion;

            GameStateMachine.Instance.GetState<InvasionState>().ExitEvent += OnInvasionStateExit;
        }

        private void OnAllowedToEndInvasion()
        {
            Show();

            endInvasionButton.onClick.AddListener(OnEndInvasionButtonClicked);
        }

        private void OnInvasionStateExit()
        {
            Hide();

            endInvasionButton.onClick.RemoveListener(OnEndInvasionButtonClicked);
        }

        private void OnEndInvasionButtonClicked()
        {
            GameStateMachine.Instance.DoTransition<AttackTransition>();
        }
    }
}