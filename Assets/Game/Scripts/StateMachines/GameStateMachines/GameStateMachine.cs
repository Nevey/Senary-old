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

            AddTransition<AnimateHQTransition, AnimateHQState>();

            AddTransition<AddInitialUnitsTransition, AddInitialUnitsState>();

            AddTransition<SelectStartPlayerTransition, SelectStartPlayerState>();
            
            AddTransition<ReceiveUnitsTransition, ReceiveUnitsState>();

            AddTransition<PlaceUnitsTransition, PlaceUnitsState>();

            AddTransition<AttackTransition, AttackState>();

            AddTransition<BattleTransition, BattleState>();

            AddTransition<CheckForWinLostTransition, CheckForWinLoseState>();

            AddTransition<IncrementPlayerTurnTransition, IncrementPlayerTurn>();
        }
    }

    // ---------- States ---------- //

    public class CreateLevelState : State { }

    public class AnimateHQState : State { }

    public class AddInitialUnitsState : State { }

    public class SelectStartPlayerState : State { }

    public class ReceiveUnitsState : State { }

    public class PlaceUnitsState : State { }

    public class AttackState : State { }

    public class BattleState : State { }

    public class CheckForWinLoseState : State { }

    public class IncrementPlayerTurn : State { }

    // --------- Transitions ---------- //

    public class CreateLevelTransition : Transition { }

    public class AnimateHQTransition : Transition { }

    public class AddInitialUnitsTransition : Transition { }

    public class SelectStartPlayerTransition : Transition { }

    public class ReceiveUnitsTransition : Transition { }

    public class PlaceUnitsTransition : Transition { }

    public class AttackTransition : Transition { }

    public class BattleTransition : Transition { }

    public class CheckForWinLostTransition : Transition { }

    public class IncrementPlayerTurnTransition : Transition { }
}