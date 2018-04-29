using System.Collections.Generic;
using CCore.Senary.Gameplay.Turns;
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

        private GridBuilder gridBuilder;

        public GenericGrid Grid { get; private set; }

        private void Awake()
        {
            GameStateMachine.Instance.GetState<CreateLevelState>().PostEnterEvent += OnCreateLevelStateEnter;

            GameStateMachine.Instance.GetState<GameOverState>().ExitEvent += OnGameOverStateExit;
            
            GameStateMachine.Instance.GetState<CheckForHQConnectionState>().PostEnterEvent += OnCheckForHQConnectionStateEnter;
        }

        private void OnDestroy()
        {
            GameStateMachine.Instance.GetState<CreateLevelState>().PostEnterEvent -= OnCreateLevelStateEnter;
            
            GameStateMachine.Instance.GetState<GameOverState>().ExitEvent -= OnGameOverStateExit;
            
            GameStateMachine.Instance.GetState<CheckForHQConnectionState>().PostEnterEvent -= OnCheckForHQConnectionStateEnter;
        }

        private void Update()
        {
            if (UnityEngine.Input.GetKey(KeyCode.A))
            {
                OnCheckForHQConnectionStateEnter();
            }
        }

        private void OnGameOverStateExit()
        {
            for (int i = 0; i < Grid.FlattenedTiles.Length; i++)
            {
                Grid.FlattenedTiles[i].ResetTile();
            }
        }

        private void OnCreateLevelStateEnter()
        {
            gridBuilder = FindObjectOfType<GridBuilder>();

            Grid = gridBuilder.Build(levelName);

            GameStateMachine.Instance.DoTransition<AnimateHQTransition>();
        }

        private void OnCheckForHQConnectionStateEnter()
        {
            for (int i = 0; i < Grid.FlattenedTiles.Length; i++)
            {
                Tile tile = Grid.FlattenedTiles[i];

                if (tile.TileType == TileType.None
                    || tile.TileType == TileType.HQ
                    || tile.OwnerState == TileOwnedState.Free)
                {
                    continue;
                }

                bool isConnected = IsTileConnectedToHQ(tile);

                if (!isConnected)
                {
                    Log("Should kill tile {0}", tile.Name);
                    
//                    tile.ClearUnits();
//                    tile.ClearOwner();
                }
            }
            
            // TODO: Kill tiles with animation, when that's done, continue...
            
            GameStateMachine.Instance.DoTransition<InvasionTransition>();
        }

        private bool IsTileConnectedToHQ(Tile tile)
        {
            List<Tile> tiles = new List<Tile>();

            return IsTileConnectedToHQ(tile, ref tiles);
        }

        private bool IsTileConnectedToHQ(Tile tile, ref List<Tile> tiles)
        {
            bool isTileConnectedToHQ = false;
            
            List<Tile> adjacentTiles = GetAdjacentTiles(tile);
            
            List<Tile> newTiles = new List<Tile>();

            for (int i = 0; i < adjacentTiles.Count; i++)
            {
                Tile adjacentTile = adjacentTiles[i];

                if (adjacentTile.TileType == TileType.HQ
                    && adjacentTile.Owner == tile.Owner)
                {
                    isTileConnectedToHQ = true;
                    
                    break;
                }

                if (tiles.Contains(adjacentTile)
                    || adjacentTile.OwnerState == TileOwnedState.Free
                    || adjacentTile.Owner != tile.Owner)
                {
                    continue;
                }

                tiles.Add(adjacentTile);

                newTiles.Add(adjacentTile);
            }

            if (!isTileConnectedToHQ)
            {
                for (int i = 0; i < newTiles.Count; i++)
                {
                    isTileConnectedToHQ = IsTileConnectedToHQ(newTiles[i], ref tiles);
                }
            }

            return isTileConnectedToHQ;
        }

        private void AddTileToList(ref List<Tile> tiles, Vector2 gridPosition)
        {
            if (!(gridPosition.x >= 0) || !(gridPosition.x < Grid.Width))
            {
                return;
            }

            if (!(gridPosition.y >= 0) || !(gridPosition.y < Grid.Height))
            {
                return;
            }
            
            Tile tile = Grid.Tiles[(int)gridPosition.x, (int)gridPosition.y];

            if (tile.TileType != TileType.None && !tiles.Contains(tile))
            {
                tiles.Add(tile);
            }
        }

        /// <summary>
        /// Returns a list of tiles adjacent to given tile, including given tile
        /// </summary>
        /// <param name="tile"></param>
        /// <returns></returns>
        public List<Tile> GetAdjacentTiles(Tile tile)
        {
            List<Tile> adjacentTiles = new List<Tile> {tile};

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
    }
}