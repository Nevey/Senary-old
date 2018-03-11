using CCore.StateMachines;

namespace CCore.Senary.StateMachines.Game
{
    public class GameStateMachine : StateMachine
    {
        public static StateMachine Instance;

        public GameStateMachine()
        {
            Instance = this;
            
            AddTransition<CreateLevelTransition, CreateLevelState>();
            
            AddTransition<PlayerInputTransition, PlayerInputState>();

            AddTransition<ThrowDiceTransition, ThrowDiceState>();
        }
    }

    // ---------- States ---------- //

    public class CreateLevelState : State { }

    public class PlayerInputState : State { }

    public class ThrowDiceState : State { }

    // --------- Transitions ---------- //

    public class CreateLevelTransition : Transition { }

    public class PlayerInputTransition : Transition { }

    public class ThrowDiceTransition : Transition { }
}