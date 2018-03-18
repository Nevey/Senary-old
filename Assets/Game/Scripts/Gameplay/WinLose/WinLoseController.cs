using System;
using CCore.Senary.StateMachines.Game;

namespace CCore.Senary.Gameplay.WinLose
{
    public class WinLoseController : MonoBehaviourSingleton<WinLoseController>
    {
        private void Awake()
        {
            GameStateMachine.Instance.GetState<CheckForWinLoseState>().EnterEvent += OnCheckForWinLostStateEnter;
        }

        private void OnCheckForWinLostStateEnter()
        {
            GameStateMachine.Instance.DoTransition<IncrementPlayerTurnTransition>();
        }
    }
}