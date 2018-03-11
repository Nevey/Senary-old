using CCore.Senary.StateMachines.Game;
using CCore.Senary.StateMachines.UI;

namespace CCore.Senary.StateMachines
{
    public class StateMachineController : MonoBehaviour
    {
        private UIStateMachine uiStateMachine = new UIStateMachine();

        private GameStateMachine gameStateMachine = new GameStateMachine();


        private void Start()
        {
            // TODO: Set start state instead of doing a transition
            // TODO: Call "statemachine.start" instead
            // gameStateMachine.DoTransition<CreateLevelTransition>();

            uiStateMachine.DoTransition<SplashTransition>();
        }
    }
}