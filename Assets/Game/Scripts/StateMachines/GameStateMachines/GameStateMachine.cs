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
        }
    }
}