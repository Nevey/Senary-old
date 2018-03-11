using System;
using CCore.Senary.StateMachines.Game;
using CCore.Senary.StateMachines.UI;

namespace CCore.Senary.StateMachines
{
    public class StateMachineController : MonoBehaviour
    {
        private UIStateMachine uiStateMachine = new UIStateMachine();

        private GameStateMachine gameStateMachine = new GameStateMachine();

        private void Awake()
        {
            
        }

        private void Start()
        {
            uiStateMachine.DoTransition<SplashTransition>();
        }
    }
}