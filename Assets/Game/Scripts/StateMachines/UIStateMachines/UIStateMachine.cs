using CCore.StateMachines;

namespace CCore.Senary.StateMachines.UI
{
    public class UIStateMachine : StateMachine
    {
        public static StateMachine Instance;

        public UIStateMachine()
        {
            Instance = this;

            AddTransition<SplashState, GameState, GameTransition>();
            
            AddTransition<GameState, SplashState, SplashTransition>();
        }
    }

    // ---------- States ---------- //

    public class SplashState : State { }

    public class GameState : State { }

    // --------- Transitions ---------- //

    public class SplashTransition : Transition { }

    public class GameTransition : Transition { }
}