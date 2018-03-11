using CCore.StateMachines;

namespace CCore.Senary.StateMachines.Game
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