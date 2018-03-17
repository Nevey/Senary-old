using System;
using System.Collections.Generic;
using CCore.Assets;
using CCore.Senary.Configs;
using CCore.Senary.Constants;
using CCore.Senary.Gameplay.Turns;
using CCore.Senary.Gameplay.Units;
using CCore.Senary.Grids;
using CCore.Senary.StateMachines.Game;
using CCore.Senary.Tiles;
using UnityEngine;

namespace CCore.Senary.Gameplay.Grid
{
    public class GridController : MonoBehaviour
    {
        // TODO: Define loading the level better
        [SerializeField] private string levelName = "Level_1";

        [SerializeField] private GameObject tilePrefab;

        [SerializeField] private TurnController turnController;

        [SerializeField] private UnitsPlacer unitsPlacer;

        private GenericGrid grid;

        public GenericGrid Grid { get { return grid; } }

        private void Awake()
        {
            GameStateMachine.Instance.GetState<CreateLevelState>().PostEnterEvent += OnCreateLevelStateEnter;

            GameStateMachine.Instance.GetState<PlaceUnitsState>().EnterEvent += OnPlaceUnitsStateEnter;

            GameStateMachine.Instance.GetState<PlaceUnitsState>().ExitEvent += OnPlaceUnitsStateExit;
        }


        private void OnDestroy()
        {
            GameStateMachine.Instance.GetState<CreateLevelState>().PostEnterEvent -= OnCreateLevelStateEnter;

            GameStateMachine.Instance.GetState<PlaceUnitsState>().EnterEvent -= OnPlaceUnitsStateEnter;

            GameStateMachine.Instance.GetState<PlaceUnitsState>().ExitEvent -= OnPlaceUnitsStateExit;
        }

        private void OnCreateLevelStateEnter()
        {
            string assetPath = String.Format(LevelConstants.levelAssetPath, levelName);

            LevelConfig levelConfig = AssetHelper.LoadAssetAtPath<LevelConfig>(assetPath);

            grid = levelConfig.Grid;

            grid.CreateTwoDimensionalGrid();
            
            for (int x = 0; x < grid.Width; x++)
            {
                for (int y = 0; y < grid.Height; y++)
                {
                    grid.Tiles[x, y].SetupTile(tilePrefab, transform, grid.Width, grid.Height);
                }
            }

            GameStateMachine.Instance.DoTransition<AnimateHQTransition>();
        }

        private void OnPlaceUnitsStateEnter()
        {
            UpdateGridTileActions();

            unitsPlacer.UnitPlacedEvent += OnUnitPlacedEvent;
        }

        private void OnPlaceUnitsStateExit()
        {
            unitsPlacer.UnitPlacedEvent -= OnUnitPlacedEvent;
        }

        private void OnUnitPlacedEvent()
        {
            UpdateGridTileActions();
        }

        private void UpdateGridTileActions()
        {
            for (int i = 0; i < grid.FlattenedTiles.Length; i++)
            {
                Tile tile = grid.FlattenedTiles[i];

                if (tile.TileType == TileType.None
                    || tile.Owner != turnController.CurrentPlayer)
                {
                    continue;
                }

                List<Tile> adjacentTiles = GetAdjacentTiles(tile);

                for (int k = 0; k < adjacentTiles.Count; k++)
                {
                    Tile adjacentTile = adjacentTiles[k];

                    if (adjacentTile.TileState == TileState.Owned)
                    {
                        if (adjacentTile.Owner != turnController.CurrentPlayer)
                        {
                            adjacentTile.SetAvailability(TileAction.AvailableForAttack);
                        }
                        else
                        {
                            adjacentTile.SetAvailability(TileAction.AvailableForReinforcement);
                        }
                    }
                    else if (adjacentTile.TileState == TileState.Free)
                    {
                        adjacentTile.SetAvailability(TileAction.AvailableForTakeOver);
                    }
                }
            }
        }

        private List<Tile> GetAdjacentTiles(Tile tile)
        {
            List<Tile> adjacentTiles = new List<Tile>();

            Vector2 gridPosition = new Vector2(
                tile.GridCoordinates.X,
                tile.GridCoordinates.Y
            );

            bool isTilted = tile.GridCoordinates.Y % 2 == 0;

            Vector2 left = gridPosition;
            left.x -= 1;

            Vector2 right = gridPosition;
            right.x += 1;

            Vector2 topLeft = gridPosition;
            topLeft.x -= isTilted ? 0 : 1;
            topLeft.y -= 1;

            Vector2 topRight = gridPosition;
            topRight.x += isTilted ? 1 : 0;
            topRight.y -= 1;

            Vector2 bottomLeft = gridPosition;
            bottomLeft.x -= isTilted ? 0 : 1;
            bottomLeft.y += 1;

            Vector2 bottomRight = gridPosition;
            bottomRight.x += isTilted ? 1 : 0;
            bottomRight.y += 1;

            AddTileToList(ref adjacentTiles, left);
            AddTileToList(ref adjacentTiles, right);
            AddTileToList(ref adjacentTiles, topLeft);
            AddTileToList(ref adjacentTiles, topRight);
            AddTileToList(ref adjacentTiles, bottomLeft);
            AddTileToList(ref adjacentTiles, bottomRight);

            return adjacentTiles;
        }

        private void AddTileToList(ref List<Tile> tiles, Vector2 gridPosition)
        {
            if (gridPosition.x >= 0 && gridPosition.x < grid.Width)
            {
                if (gridPosition.y >= 0 && gridPosition.y < grid.Height)
                {
                    Tile tile = grid.Tiles[(int)gridPosition.x, (int)gridPosition.y];

                    if (tile.TileType != TileType.None)
                    {
                        tiles.Add(tile);
                    }
                }
            }
        }
    }
}