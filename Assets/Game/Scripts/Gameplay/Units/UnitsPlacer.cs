using System;
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
        [SerializeField] private GridController gridController;

        [SerializeField] private UnitsReceiver unitsReceiver;

        [SerializeField] private TurnController turnController;

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
            for (int i = 0; i < gridController.Grid.FlattenedTiles.Length; i++)
            {
                Tile tile = gridController.Grid.FlattenedTiles[i];

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
        }

        private void OnPlaceUnitsStateExit()
        {
            PlayerInput.Instance.TapEvent -= OnTap;
        }

        private List<Tile> GetAvailableTiles()
        {
            List<Tile> tiles = new List<Tile>();

            for (int i = 0; i < GridController.Instance.Grid.FlattenedTiles.Length; i++)
            {
                Tile tile = GridController.Instance.Grid.FlattenedTiles[i];

                if (tile.TileType == TileType.None
                    || tile.Owner != turnController.CurrentPlayer)
                {
                    continue;
                }

                List<Tile> adjacentTiles = GridController.Instance.GetAdjacentTiles(tile);

                for (int k = 0; k < adjacentTiles.Count; k++)
                {
                    Tile adjacentTile = adjacentTiles[k];

                    if (adjacentTile.TileOwnedState == TileOwnedState.Owned)
                    {
                        if (adjacentTile.Owner == turnController.CurrentPlayer)
                        {
                            // adjacentTile.SetTileGameState(TileGameState.AvailableAsTarget);
                            adjacentTile.SetTileGameState(TileGameState.AvailableForReinforcement);

                            if (!tiles.Contains(adjacentTile))
                            {
                                tiles.Add(adjacentTile);
                            }
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
            bool isFinished = false;

            for (int i = 0; i < availableTiles.Count; i++)
            {
                Tile tile = availableTiles[i];

                if (tile.TileInput.TapTile(position))
                {
                    bool addUnitsSuccess = tile.AddUnits(1, turnController.CurrentPlayer);

                    if (addUnitsSuccess)
                    {
                        Log("Added one unit to tapped tile. Unit count is now {0}.", tile.UnitCount);

                        isFinished = unitsReceiver.DecrementNewUnitCount();
                    }
                }
            }

            for (int i = 0; i < availableTiles.Count; i++)
            {
                availableTiles[i].SetTileGameState(TileGameState.NotAvailable);
            }

            if (isFinished)
            {
                GameStateMachine.Instance.DoTransition<AttackTransition>();
            }
            else
            {
                availableTiles = GetAvailableTiles();
            }
        }
    }
}