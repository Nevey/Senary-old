using System;
using CCore.Senary.StateMachines.Game;
using CCore.Senary.StateMachines.UI;
using CCore.UI;
using UnityEngine;
using UnityEngine.UI;

namespace CCore.Senary.UI
{
    public class GameView : UIView
    {
        [SerializeField] private Button throwButton;
        
        protected override void Setup()
        {
            UIStateMachine.Instance.GetState<GameState>().EnterEvent += OnGameStateEnter;

            GameStateMachine.Instance.GetState<PlayerInputState>().EnterEvent += OnPlayerInputStateEnter;

            GameStateMachine.Instance.GetState<PlayerInputState>().ExitEvent += OnPlayerInputStateExit;
        }

        private void OnGameStateEnter()
        {
            Show();
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