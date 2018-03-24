using System;
using CCore.Senary.StateMachines.Game;
using CCore.UI;

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
            Show();
        }

        private void OnGameOverStateExit()
        {
            Hide();
        }
    }
}