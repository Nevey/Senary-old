using CCore.StateMachines;

namespace CCore.Senary.StateMachines.UI
{
    public class UIStateMachine : StateMachineSingleton<UIStateMachine>
    {
        public UIStateMachine()
        {
            AddTransition<SplashTransition, SplashState>();
        }
    }

    public class SplashState : State { }

    public class SplashTransition : Transition { }
}