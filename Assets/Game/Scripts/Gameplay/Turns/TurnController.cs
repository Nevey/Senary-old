using System;
using System.Collections.Generic;
using CCore.Senary.Gameplay.Grid;
using CCore.Senary.Players;
using CCore.Senary.StateMachines.Game;
using CCore.Senary.Tiles;
using UnityEngine;

namespace CCore.Senary.Gameplay.Turns
{
    public class TurnController : MonoBehaviourSingleton<TurnController>
    {
        [SerializeField] private GridController gridController;

        private List<Player> playerList = new List<Player>();

        private Player currentPlayer;

        public List<Player> PlayerList { get { return playerList; } }

        public Player CurrentPlayer { get { return currentPlayer; } }

        public int CurrentPlayerNumber { get { return playerList.IndexOf(currentPlayer) + 1; } }

        public event Action TurnStartedEvent;

        private void Awake()
        {
            GameStateMachine.Instance.GetState<SelectStartPlayerState>().EnterEvent += OnSelectStartPlayerStateEnter;

            GameStateMachine.Instance.GetState<IncrementPlayerTurnState>().EnterEvent += OnIncrementPlayerTurnStateEnter;
        }

        private void OnDestroy()
        {
            GameStateMachine.Instance.GetState<SelectStartPlayerState>().EnterEvent -= OnSelectStartPlayerStateEnter;

            GameStateMachine.Instance.GetState<IncrementPlayerTurnState>().EnterEvent -= OnIncrementPlayerTurnStateEnter;
        }

        private void OnSelectStartPlayerStateEnter()
        {
            Log("Select Start Player...");

            // TODO: Shuffle player list
            playerList = GetActivePlayers();

            Log("Player List Populated, {0} players were found", playerList.Count);

            int randomIndex = UnityEngine.Random.Range(0, playerList.Count);

            GiveTurnToPlayer(playerList[randomIndex]);
        }

        private void OnIncrementPlayerTurnStateEnter()
        {
            int newPlayerIndex = playerList.IndexOf(currentPlayer) + 1;

            if (newPlayerIndex == playerList.Count)
            {
                newPlayerIndex = 0;
            }

            GiveTurnToPlayer(playerList[newPlayerIndex]);
        }

        private List<Player> GetActivePlayers()
        {
            List<Player> playerList = new List<Player>();

            for (int i = 0; i < gridController.Grid.FlattenedTiles.Length; i++)
            {
                Tile tile = gridController.Grid.FlattenedTiles[i];

                if (tile.TileType == TileType.HQ && tile.Owner.PlayerID.ID != -1)
                {
                    if (!playerList.Contains(tile.Owner))
                    {
                        playerList.Add(tile.Owner);
                    }
                }
            }

            return playerList;
        }

        private void GiveTurnToPlayer(Player player)
        {
            currentPlayer = player;

            Log("Player {0}'s Turn!", CurrentPlayerNumber);

            if (TurnStartedEvent != null)
            {
                TurnStartedEvent();
            }

            GameStateMachine.Instance.DoTransition<AttackTransition>();
        }

        public int GetPlayerNumber(Player player)
        {
            return playerList.IndexOf(player) + 1;
        }

        public void RemovePlayerFromGame(Player player)
        {
            playerList.Remove(player);
        }
    }
}