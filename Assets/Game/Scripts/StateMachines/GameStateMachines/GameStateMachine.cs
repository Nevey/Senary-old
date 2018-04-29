using CCore.StateMachines;
using UnityEngine.Analytics;

namespace CCore.Senary.StateMachines.Game
{
    public class GameStateMachine : StateMachine
    {
        public static StateMachine Instance;

        public GameStateMachine()
        {
            Instance = this;

            AddTransition<CreateLevelState, AnimateHQState, AnimateHQTransition>();

            AddTransition<AnimateHQState, AddInitialUnitsState, AddInitialUnitsTransition>();

            AddTransition<AddInitialUnitsState, SelectStartPlayerState, SelectStartPlayerTransition>();

            
            AddTransition<SelectStartPlayerState, AttackState, AttackTransition>();
            
            AddTransition<IncrementPlayerTurnState, AttackState, AttackTransition>();
            
            AddTransition<DefenderWinBattleState, AttackState, AttackTransition>();
            
            AddTransition<InvasionState, AttackState, AttackTransition>();
            

            AddTransition<AttackState, BattleState, BattleTransition>();
            
            AddTransition<BattleState, AttackerWinBattleState, AttackerWinBattleTransition>();
            
            AddTransition<BattleState, DefenderWinBattleState, DefenderWinBattleTransition>();
            
            AddTransition<AttackerWinBattleState, CheckForWinLoseState, CheckForWinLoseTransition>();
            
            AddTransition<CheckForWinLoseState, CheckForHQConnectionState, CheckForHQConnectionTransition>();
            
            AddTransition<CheckForHQConnectionState, InvasionState, InvasionTransition>();
            
            
            AddTransition<AttackState, ReceiveUnitsState, ReceiveUnitsTransition>();
            
            AddTransition<InvasionState, ReceiveUnitsState, ReceiveUnitsTransition>();
            
            
            AddTransition<ReceiveUnitsState, PlaceUnitsState, PlaceUnitsTransition>();
            
            AddTransition<PlaceUnitsState, IncrementPlayerTurnState, IncrementPlayerTurnTransition>();
            
            AddTransition<CheckForWinLoseState, GameOverState, GameOverTransition>();
            
            AddTransition<GameOverState, IdleState, IdleTransition>();
        }
    }

    // ---------- States ---------- //

    public class CreateLevelState : State { }

    public class AnimateHQState : State { }

    public class AddInitialUnitsState : State { }

    public class SelectStartPlayerState : State { }

    public class AttackState : State { }

    public class BattleState : State { }

    public class AttackerWinBattleState : State { }

    public class DefenderWinBattleState : State { }
    
    public class CheckForHQConnectionState : State { }

    public class CheckForWinLoseState : State { }

    public class InvasionState : State { }

    public class ReceiveUnitsState : State { }

    public class PlaceUnitsState : State { }

    public class IncrementPlayerTurnState : State { }

    public class GameOverState : State { }

    public class IdleState : State { }

    // --------- Transitions ---------- //

    public class CreateLevelTransition : Transition { }

    public class AnimateHQTransition : Transition { }

    public class AddInitialUnitsTransition : Transition { }

    public class SelectStartPlayerTransition : Transition { }

    public class AttackTransition : Transition { }

    public class BattleTransition : Transition { }

    public class AttackerWinBattleTransition : Transition { }

    public class DefenderWinBattleTransition : Transition { }
    
    public class CheckForHQConnectionTransition : Transition { }

    public class CheckForWinLoseTransition : Transition { }

    public class InvasionTransition : Transition { }

    public class ReceiveUnitsTransition : Transition { }

    public class PlaceUnitsTransition : Transition { }

    public class IncrementPlayerTurnTransition : Transition { }

    public class GameOverTransition : Transition { }

    public class IdleTransition : Transition { }
}