using CCore.StateMachines;

namespace CCore.Senary.StateMachines
{
    public class GameStateMachine : StateMachine
    {
        public GameStateMachine()
        {
            AddTransition<CreateLevelTransition, CreateLevelState>();
            
            AddTransition<PlayerInputTransition, PlayerInputState>();
        }
    }
}