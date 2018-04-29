using System;
using CCore.Senary.StateMachines.Game;
using CCore.Senary.StateMachines.UI;

namespace CCore.Senary.StateMachines
{
    public class StateMachineController : MonoBehaviourSingleton<StateMachineController>
    {
        [NonSerialized]
        private GameStateMachine gameStateMachine = new GameStateMachine();
        
        [NonSerialized]
        private UIStateMachine uiStateMachine = new UIStateMachine();

        private void Start()
        {
            uiStateMachine.Start<SplashState>();
        }

        public void StartGameStateMachine()
        {
            gameStateMachine.Start<CreateLevelState>();
        }

        public void StopGameStateMachine()
        {
            gameStateMachine.Stop();
        }
    }
}