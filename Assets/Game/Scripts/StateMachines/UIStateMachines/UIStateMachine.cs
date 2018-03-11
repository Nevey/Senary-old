using CCore.StateMachines;

namespace CCore.Senary.StateMachines.UI
{
    public class UIStateMachine : StateMachine
    {
        public static StateMachine Instance;

        public UIStateMachine()
        {
            Instance = this;
            
            AddTransition<SplashTransition, SplashState>();
        }
    }

    public class SplashState : State { }

    public class SplashTransition : Transition { }
}