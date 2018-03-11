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

            AddTransition<GameTransition, GameState>();

            AddTransition<PauseGameTransition, PauseGameState>();
        }
    }

    // ---------- States ---------- //

    public class SplashState : State { }

    public class GameState : State { }

    public class PauseGameState : State { }

    // --------- Transitions ---------- //

    public class SplashTransition : Transition { }

    public class GameTransition : Transition { }

    public class PauseGameTransition : Transition { }
}