using CCore.Senary.StateMachines.UI;
using CCore.UI;

namespace CCore.Senary.UI
{
    public class GameView : UIView
    {
        protected override void Setup()
        {
            UIStateMachine.Instance.GetState<GameState>().EnterEvent += OnGameStateEnter;
        }

        private void OnGameStateEnter()
        {
            Show();
        }
    }
}