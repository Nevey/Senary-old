namespace CCore.Senary.StateMachines
{
    public class StateMachineController : MonoBehaviour
    {
        private GameStateMachine gameStateMachine;

        private void Awake()
        {
            // TODO: Set start state instead of doing a transition
            gameStateMachine = new GameStateMachine();

            // TODO: Call "statemachine.start" instead
            gameStateMachine.DoTransition<PlayerInputTransition>();
        }
    }
}