using CCore.Senary.Input;
using CCore.Senary.StateMachines.Game;
using CCore.Senary.StateMachines.UI;
using CCore.UI;
using UnityEngine;

namespace CCore.Senary.UI
{
    public class GameOverView : UIView
    {
        protected override void Setup()
        {
            GameStateMachine.Instance.GetState<GameOverState>().EnterEvent += OnGameOverStateEnter;

            GameStateMachine.Instance.GetState<GameOverState>().ExitEvent += OnGameOverStateExit;
        }

        private void OnGameOverStateEnter()
        {
            PlayerInput.Instance.TapEvent += OnTap;
            
            Show();
        }

        private void OnGameOverStateExit()
        {
            PlayerInput.Instance.TapEvent -= OnTap;

            Hide();
        }

        private void OnTap(Vector2 position)
        {
            GameStateMachine.Instance.DoTransition<IdleTransition>();

            UIStateMachine.Instance.DoTransition<SplashTransition>();
        }
    }
}