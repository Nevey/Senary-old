using System;
using CCore.Assets;
using CCore.Senary.Configs;
using CCore.Senary.Grids;
using CCore.Senary.Players;
using CCore.Senary.Tiles;
using UnityEditor;
using UnityEngine;

namespace CCore.Senary.Editors
{
    public class LevelEditorController
    {
        private const string levelAssetPath = "Assets/Game/Configs/Levels/{0}.asset";

        private Texture2D groundHexTexture;

        private Texture2D hqHexTexture;

        private GenericGrid<EditorTile> grid;

        private Player[] players;

        private Color[] playerColors;

        public Texture2D GroundHexTexture { get { return groundHexTexture; } }

        public Texture2D HqHexTexture { get { return hqHexTexture; } }

        public GenericGrid<EditorTile> Grid { get { return grid; } }

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
        
        private EditorTile GetClosestTile(Vector2 position, Rect windowRect)
        {
            float distance = 10000f;

            EditorTile closestTile = null;

            for (int x = 0; x < grid.Width; x++)
            {
                for (int y = 0; y < grid.Height; y++)
                {
                    EditorTile tile = grid.Tiles[x, y];

                    Vector2 centerPosition = tile.CenterPosition;

                    // TODO: Half window width should become a "grid offset" variable
                    centerPosition.x += windowRect.width * 0.5f;

                    float localDistance = Vector2.Distance(centerPosition, position);

                    if (localDistance < distance)
                    {
                        closestTile = tile;

                        distance = localDistance;
                    }
                }
            }

            if (distance > groundHexTexture.height * 0.5f)
            {
                return null;
            }

            return closestTile;
        }

        public void CreateGrid(int gridWidth, int gridHeight)
        {
            grid = new GenericGrid<EditorTile>(gridWidth, gridHeight);

            for (int x = 0; x < grid.Width; x++)
            {
                for (int y = 0; y < grid.Height; y++)
                {
                    Rect rect = new Rect(
                        groundHexTexture.width * x,
                        (groundHexTexture.height * .75f) * y,
                        groundHexTexture.width,
                        groundHexTexture.height
                    );

                    // Adding some offsets for positioning reasons
                    rect.x -= rect.width * (gridWidth * 0.5f);
                    rect.x -= rect.width * 0.25f;

                    // TODO: Define this magic number more nicely
                    rect.y += 180f;

                    if (y % 2 == 0)
                    {
                        rect.x += groundHexTexture.width * .5f;
                    }

                    grid.Tiles[x, y].SetRect(rect);
                }
            }
        }

        public void UpdateTileType(Vector2 position, Rect windowRect)
        {
            if (grid == null)
            {
                return;
            }

            EditorTile tile = GetClosestTile(position, windowRect);

            if (tile != null)
            {
                tile.IncrementTileType();
            }

        }

        public void UpdateTileOwner(Vector2 position, Rect windowRect)
        {
            if (grid == null || players == null || players.Length == 0)
            {
                return;
            }

            EditorTile tile = GetClosestTile(position, windowRect);

            if (tile == null)
            {
                return;
            }

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

        public void ClearTileOwner(Vector2 position, Rect windowRect)
        {
            if (grid == null)
            {
                return;
            }

            EditorTile tile = GetClosestTile(position, windowRect);

            if (tile == null)
            {
                return;
            }

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
                EditorTile tile = grid.FlattenedTiles[i];

                if (tile.TileType == TileType.HQ)
                {
                    maxPlayerCount++;
                }
            }

            return maxPlayerCount > playerColors.Length ? playerColors.Length : maxPlayerCount;
        }
        
        public void SaveLevel(string levelName)
        {
            LevelConfig levelConfig = ScriptableObject.CreateInstance<LevelConfig>();

            levelConfig.SetLevelData(grid);

            string assetPath = String.Format(levelAssetPath, levelName);

            AssetHelper.CreateAsset<LevelConfig>(levelConfig, assetPath);
        }

        public void LoadLevel(string levelName)
        {
            string assetPath = String.Format(levelAssetPath, levelName);

            LevelConfig levelConfig = AssetHelper.LoadAssetAtPath<LevelConfig>(assetPath);

            grid = levelConfig.Grid;
        }
    }
}