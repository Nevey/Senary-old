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
    public class GridController : MonoBehaviourSingleton<GridController>
    {
        // TODO: Define loading the level better
        [SerializeField] private string levelName = "Level_1";

        private GenericGrid grid;

        private GridBuilder gridBuilder;

        public GenericGrid Grid { get { return grid; } }

        private void Awake()
        {
            GameStateMachine.Instance.GetState<CreateLevelState>().PostEnterEvent += OnCreateLevelStateEnter;

            GameStateMachine.Instance.GetState<GameOverState>().ExitEvent += OnGameOverStateExit;
        }

        private void OnGameOverStateExit()
        {
            for (int i = 0; i < grid.FlattenedTiles.Length; i++)
            {
                grid.FlattenedTiles[i].ResetTile();
            }
        }

        private void OnDestroy()
        {
            GameStateMachine.Instance.GetState<CreateLevelState>().PostEnterEvent -= OnCreateLevelStateEnter;
        }

        private void OnCreateLevelStateEnter()
        {
            gridBuilder = FindObjectOfType<GridBuilder>();

            grid = gridBuilder.Build(levelName);

            GameStateMachine.Instance.DoTransition<AnimateHQTransition>();
        }

        /// <summary>
        /// Returns a list of tiles adjacent to given tile, including given tile
        /// </summary>
        /// <param name="tile"></param>
        /// <returns></returns>
        public List<Tile> GetAdjacentTiles(Tile tile)
        {
            List<Tile> adjacentTiles = new List<Tile>();

            adjacentTiles.Add(tile);

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

                    if (tile.TileType != TileType.None && !tiles.Contains(tile))
                    {
                        tiles.Add(tile);
                    }
                }
            }
        }
    }
}