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

            AddTransition<SelectStartPlayerTransition, SelectStartPlayerState>();
            
            AddTransition<ReceiveUnitsTransition, ReceiveUnitsState>();

            AddTransition<ThrowDiceTransition, ThrowDiceState>();
        }
    }

    // ---------- States ---------- //

    public class CreateLevelState : State { }

    public class SelectStartPlayerState : State { }

    public class ReceiveUnitsState : State { }

    public class ThrowDiceState : State { }

    // --------- Transitions ---------- //

    public class CreateLevelTransition : Transition { }

    public class SelectStartPlayerTransition : Transition { }

    public class ReceiveUnitsTransition : Transition { }

    public class ThrowDiceTransition : Transition { }
}