namespace CCore.Senary.StateMachines
{
    public class StateMachineController : MonoBehaviour
    {
        private GameStateMachine gameStateMachine;

        private void Awake()
        {
            gameStateMachine = new GameStateMachine();

            gameStateMachine.DoTransition<PlayerInputTransition>();
        }
    }
}