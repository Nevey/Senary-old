namespace CCore.Senary.StateMachines
{
    public class StateMachineController : MonoBehaviour
    {
        private GameStateMachine gameStateMachine = new GameStateMachine();

        private void Awake()
        {
        }

        private void Start()
        {
            // TODO: Set start state instead of doing a transition
            // TODO: Call "statemachine.start" instead
            gameStateMachine.DoTransition<CreateLevelTransition>();
        }
    }
}