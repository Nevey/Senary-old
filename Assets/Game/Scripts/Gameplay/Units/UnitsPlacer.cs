using System;
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

        private void Awake()
        {
            GameStateMachine.Instance.GetState<PlaceUnitsState>().EnterEvent += OnPlaceUnitsStateEnter;

            GameStateMachine.Instance.GetState<PlaceUnitsState>().ExitEvent += OnPlaceUnitsStateExit;
        }

        private void OnPlaceUnitsStateEnter()
        {
            // Tap a tile to add a unit
            // units receiver decrement units
            PlayerInput.Instance.TapEvent += OnTap;
        }

        private void OnPlaceUnitsStateExit()
        {
            PlayerInput.Instance.TapEvent -= OnTap;
        }

        private void OnTap(Vector2 position)
        {
            for (int i = 0; i < gridController.Grid.FlattenedTiles.Length; i++)
            {
                Tile tile = gridController.Grid.FlattenedTiles[i];

                if (tile.TileType == TileType.None)
                {
                    continue;
                }

                TileInput tileInput = tile.TileMesh.GetComponent<TileInput>();

                if (tileInput.TapTile(position))
                {
                    // TODO: Only able to tap tiles which are adjacent to owned tiles
                    if (tile.Owner == turnController.CurrentPlayer
                        || tile.Owner.PlayerID.ID == -1)
                    {
                        tile.AddUnits(1);

                        Log("Added one unit to tapped tile. Unit count is now {0}.", tile.UnitCount);
                    }
                }
            }
        }
    }
}