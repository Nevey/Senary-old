using System;
using CCore.Senary.StateMachines.Game;
using CCore.UI;

namespace CCore.Senary.UI
{
    public class ThrowDiceView : UIView
    {
        protected override void Setup()
        {
            GameStateMachine.Instance.GetState<BattleState>().EnterEvent += OnBattleStateEnter;
        }

        private void OnBattleStateEnter()
        {
            Show();
        }
    }
}