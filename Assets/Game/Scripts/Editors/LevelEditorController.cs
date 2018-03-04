using System;
using CCore.Assets;
using CCore.Senary.Grids;
using CCore.Senary.Players;
using CCore.Senary.Tiles;
using UnityEngine;

namespace CCore.Senary.Editors
{
    public class LevelEditorController
    {
        private Texture2D groundHexTexture;

        private Texture2D hqHexTexture;

        private GenericGrid<Tile2D> grid;

        private Player[] players;

        private Color[] playerColors;

        public Texture2D GroundHexTexture { get { return groundHexTexture; } }

        public Texture2D HqHexTexture { get { return hqHexTexture; } }

        public GenericGrid<Tile2D> Grid { get { return grid; } }

        public LevelEditorController()
        {
            LoadTextures();

            SetupPlayerColors();
        }

        private void LoadTextures()
        {
            groundHexTexture = AssetHelper.LoadAsset<Texture2D>("groundHex");

            hqHexTexture = AssetHelper.LoadAsset<Texture2D>("hqHex");
        }

        private void SetupPlayerColors()
        {
            playerColors = new Color[7];

            playerColors[0] = Color.red;
            playerColors[1] = Color.blue;
            playerColors[2] = Color.cyan;
            playerColors[3] = Color.magenta;
            playerColors[4] = Color.green;
            playerColors[5] = Color.yellow;
            playerColors[6] = Color.grey;
        }
        
        private Tile2D GetClosestTile(Vector2 position)
        {
            float distance = 10000f;

            Tile2D closestTile = null;

            for (int x = 0; x < grid.Width; x++)
            {
                for (int y = 0; y < grid.Height; y++)
                {
                    Tile2D tile = grid.Tiles[x, y];

                    float localDistance = Vector2.Distance(tile.CenterPosition, position);

                    if (localDistance < distance)
                    {
                        closestTile = tile;

                        distance = localDistance;
                    }
                }
            }

            return closestTile;
        }

        public void CreateGrid(int gridWidth, int gridHeight)
        {
            grid = new GenericGrid<Tile2D>(gridWidth, gridHeight);

            float startX = 20f;
            float startY = 150f;

            for (int x = 0; x < grid.Width; x++)
            {
                for (int y = 0; y < grid.Height; y++)
                {
                    Rect rect = new Rect(
                        startX + groundHexTexture.width * x,
                        startY + (groundHexTexture.height * .75f) * y,
                        groundHexTexture.width,
                        groundHexTexture.height
                    );

                    if (y % 2 == 0)
                    {
                        rect.x += groundHexTexture.width * .5f;
                    }

                    grid.Tiles[x, y].SetRect(rect);
                }
            }
        }

        public void UpdateTileType(Vector2 position)
        {
            if (grid == null)
            {
                return;
            }

            Tile2D tile = GetClosestTile(position);

            tile.IncrementTileType();
        }

        public void UpdateTileOwner(Vector2 position)
        {
            if (grid == null || players == null || players.Length == 0)
            {
                return;
            }

            Tile2D tile = GetClosestTile(position);

            if (tile.Owner == null)
            {
                tile.SetOwner(players[0]);

                return;
            }

            // Immediately increment index with + 1
            int playerIndex = Array.IndexOf(players, tile.Owner) + 1;

            playerIndex = playerIndex == players.Length ? 0 : playerIndex;

            tile.SetOwner(players[playerIndex]);
        }

        public void ClearTileOwner(Vector2 position)
        {
            if (grid == null)
            {
                return;
            }

            Tile2D tile = GetClosestTile(position);

            tile.ClearOwner();
        }

        public void UpdateAvailablePlayers()
        {
            int playerCount = GetMaxPlayerCount();

            if (players != null && players.Length == playerCount)
            {
                return;
            }

            players = new Player[playerCount];

            for (int i = 0; i < playerCount; i++)
            {
                PlayerID playerID = new PlayerID(i, "player_" + i, playerColors[i]);

                Player player = new Player(playerID);

                players[i] = player;
            }
        }

        public int GetMaxPlayerCount()
        {
            if (grid == null)
            {
                return 0;
            }

            int maxPlayerCount = 0;

            // Get the amount of HQ's in the grid
            for (int i = 0; i < grid.FlattenedTiles.Length; i++)
            {
                Tile2D tile = grid.FlattenedTiles[i];

                if (tile.TileType == TileType.HQ)
                {
                    maxPlayerCount++;
                }
            }

            return maxPlayerCount > playerColors.Length ? playerColors.Length : maxPlayerCount;
        }
    }
}