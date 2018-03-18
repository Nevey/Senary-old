using System;
using CCore.Senary.StateMachines.Game;
using CCore.Senary.StateMachines.UI;

namespace CCore.Senary.StateMachines
{
    public class StateMachineController : MonoBehaviour
    {
        [NonSerialized]
        private GameStateMachine gameStateMachine = new GameStateMachine();
        
        [NonSerialized]
        private UIStateMachine uiStateMachine = new UIStateMachine();
    }
}