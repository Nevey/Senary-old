using System;
using System.Collections.Generic;
using CCore.Senary.Gameplay.Turns;
using CCore.Senary.Players;
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
            List<Player> playerList = TurnController.Instance.PlayerList;

            for (int i = playerList.Count - 1; i >= 0; i--)
            {
                Player player = playerList[i];

                // If a player has no more HQ's, player has lost
                if (player.OwnedHQCount == 0)
                {
                    TurnController.Instance.RemovePlayerFromGame(player);
                }
            }

            if (playerList.Count > 1)
            {
                GameStateMachine.Instance.DoTransition<IncrementPlayerTurnTransition>();
            }
            else
            {
                GameStateMachine.Instance.DoTransition<GameOverTransition>();
            }
        }
    }
}