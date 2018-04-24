using System;
using System.Collections;
using System.Collections.Generic;
using CCore.Senary.Gameplay.Grid;
using CCore.Senary.Gameplay.Tiles;
using CCore.Senary.Gameplay.Turns;
using CCore.Senary.Input;
using CCore.Senary.StateMachines.Game;
using CCore.Senary.Tiles;
using UnityEngine;

namespace CCore.Senary.Gameplay.Units
{
    public class UnitsPlacer : MonoBehaviour
    {
        private List<Tile> availableTiles;

        private void Awake()
        {
            GameStateMachine.Instance.GetState<AddInitialUnitsState>().EnterEvent += OnAddInitialUnitsStateEnter;
            
            GameStateMachine.Instance.GetState<PlaceUnitsState>().EnterEvent += OnPlaceUnitsStateEnter;

            GameStateMachine.Instance.GetState<PlaceUnitsState>().ExitEvent += OnPlaceUnitsStateExit;
        }

        private void OnDestroy()
        {
            GameStateMachine.Instance.GetState<AddInitialUnitsState>().EnterEvent -= OnAddInitialUnitsStateEnter;
            
            GameStateMachine.Instance.GetState<PlaceUnitsState>().EnterEvent -= OnPlaceUnitsStateEnter;

            GameStateMachine.Instance.GetState<PlaceUnitsState>().ExitEvent -= OnPlaceUnitsStateExit;
        }

        private void OnAddInitialUnitsStateEnter()
        {
            for (int i = 0; i < GridController.Instance.Grid.FlattenedTiles.Length; i++)
            {
                Tile tile = GridController.Instance.Grid.FlattenedTiles[i];

                if (tile.TileType == TileType.HQ && tile.Owner.PlayerID.ID != -1)
                {
                    tile.AddUnits(1, tile.Owner);
                }
            }

            GameStateMachine.Instance.DoTransition<SelectStartPlayerTransition>();
        }

        private void OnPlaceUnitsStateEnter()
        {
            PlayerInput.Instance.TapEvent += OnTap;

            availableTiles = GetAvailableTiles();

            if (availableTiles.Count == 0)
            {
                StartCoroutine(WaitOneFrame(() =>
                {
                    GameStateMachine.Instance.DoTransition<IncrementPlayerTurnTransition>();
                }));
            }
        }

        private void OnPlaceUnitsStateExit()
        {
            PlayerInput.Instance.TapEvent -= OnTap;

            ResetAvailableTiles();
        }

        private List<Tile> GetAvailableTiles()
        {
            List<Tile> tiles = new List<Tile>();

            for (int i = 0; i < GridController.Instance.Grid.FlattenedTiles.Length; i++)
            {
                Tile tile = GridController.Instance.Grid.FlattenedTiles[i];

                if (tile.TileType == TileType.None
                    || tile.Owner != TurnController.Instance.CurrentPlayer)
                {
                    continue;
                }

                List<Tile> adjacentTiles = GridController.Instance.GetAdjacentTiles(tile);

                for (int k = 0; k < adjacentTiles.Count; k++)
                {
                    Tile adjacentTile = adjacentTiles[k];

                    if (adjacentTile.TileOwnedState == TileOwnedState.Owned
                        && adjacentTile.Owner == TurnController.Instance.CurrentPlayer
                        && adjacentTile.UnitCount < 5)
                    {
                        adjacentTile.SetTileGameState(TileGameState.AvailableForReinforcement);

                        if (!tiles.Contains(adjacentTile))
                        {
                            tiles.Add(adjacentTile);
                        }
                    }
                    else if (adjacentTile.TileOwnedState == TileOwnedState.Free)
                    {
                        adjacentTile.SetTileGameState(TileGameState.AvailableForTakeOver);

                        if (!tiles.Contains(adjacentTile))
                        {
                            tiles.Add(adjacentTile);
                        }
                    }
                }
            }

            return tiles;
        }

        private void OnTap(Vector2 position)
        {
            int reinforcementsCount = 0;

            for (int i = 0; i < availableTiles.Count; i++)
            {
                Tile tile = availableTiles[i];

                if (!tile.TileInput.TapTile(position))
                {
                    continue;
                }
                
                bool addUnitsSuccess = tile.AddUnits(1, TurnController.Instance.CurrentPlayer);

                if (!addUnitsSuccess)
                {
                    continue;
                }
                
                Log("Added one unit to tapped tile. Unit count is now {0}.", tile.UnitCount);

                reinforcementsCount = UnitsReceiver.Instance.DecrementNewUnitCount();
            }

            // Reset available tiles after placing a unit
            ResetAvailableTiles();

            // Get new available tiles
            availableTiles = GetAvailableTiles();

            if (reinforcementsCount == 0 || availableTiles.Count == 0)
            {
                GameStateMachine.Instance.DoTransition<IncrementPlayerTurnTransition>();
            }
        }

        private void ResetAvailableTiles()
        {
            for (int i = 0; i < availableTiles.Count; i++)
            {
                availableTiles[i].SetTileGameState(TileGameState.NotAvailable);
            }
        }

        // TODO: Move this to it's own class
        private IEnumerator WaitOneFrame(Action callback)
        {
            yield return new WaitForEndOfFrame();

            callback();
        }
    }
}