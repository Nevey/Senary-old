using System;
using CCore.Assets;
using CCore.Editors;
using CCore.Input;
using CCore.Senary.Grids;
using CCore.Senary.Players;
using CCore.Senary.Tiles;
using UnityEditor;
using UnityEngine;

namespace CCore.Senary.Editors
{
    public class LevelEditorWindow : BaseEditorWindow
    {
        private int gridWidth;

        private int gridHeight;

        private Texture2D groundHexTexture;

        private Texture2D hqHexTexture;

        private GenericGrid<Tile2D> grid;

        private Player[] players;

        private Color[] playerColors;
        
        [MenuItem("Senary/LevelEditor")]
        public static void ShowWindow()
        {
            ShowWindow<LevelEditorWindow>();
        }

        private void Awake()
        {
            titleContent = new GUIContent("v0.0.1");

            LoadTextures();

            playerColors = new Color[7];

            playerColors[0] = Color.red;
            playerColors[1] = Color.blue;
            playerColors[2] = Color.cyan;
            playerColors[3] = Color.magenta;
            playerColors[4] = Color.green;
            playerColors[5] = Color.yellow;
            playerColors[6] = Color.grey;
        }

        protected override void Update()
        {
            // UpdateAvailablePlayers();
        }

        protected override void OnGUI()
        {
            base.OnGUI();
            
            // TODO: Define min size after level grid drawing is functioning
            minSize = new Vector2(300f, 200f);

            DrawTopHeader();

            DrawLevelProperties();

            DrawPlayersInfo();

            DrawLevelGrid();
        }
        
        protected override void OnMouseDown(Vector2 position, MouseButton mouseButton)
        {
            if (mouseButton == MouseButton.Left)
            {
                UpdateTileType(position);
            }

            UpdateAvailablePlayers();

            if (mouseButton == MouseButton.Right)
            {
                UpdateTileOwner(position);
            }

            Repaint();
        }

        protected override void OnMouseDrag(Vector2 position, MouseButton mouseButton)
        {
            // throw new NotImplementedException();
        }

        protected override void OnMouseUp(Vector2 position, MouseButton mouseButton)
        {
            // throw new NotImplementedException();
        }

        private void LoadTextures()
        {
            groundHexTexture = AssetHelper.LoadAsset<Texture2D>("groundHex");

            hqHexTexture = AssetHelper.LoadAsset<Texture2D>("hqHex");
        }

        private void CreateGrid()
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

        private void UpdateTileType(Vector2 position)
        {
            if (grid == null)
            {
                return;
            }

            Tile2D tile = GetClosestTile(position);

            tile.IncrementTileType();
        }

        private void UpdateTileOwner(Vector2 position)
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

        private int GetMaxPlayerCount()
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

        private void UpdateAvailablePlayers()
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

        private void DrawTopHeader()
        {
            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();

            GUILayout.FlexibleSpace();

            GUILayout.Label("SENARY LEVEL EDITOR", EditorStyles.largeLabel);

            GUILayout.FlexibleSpace();

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            EditorGUILayout.Space();
        }

        private void DrawLevelProperties()
        {
            gridWidth = EditorGUILayout.IntSlider("Grid Width", gridWidth, 4, 20);

            gridHeight = EditorGUILayout.IntSlider("Grid Height", gridHeight, 4, 20);

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();

            GUILayout.FlexibleSpace();

            if (GUILayout.Button("CREATE GRID"))
            {
                CreateGrid();
            }

            if (GUILayout.Button("SAVE LEVEL"))
            {
                // TODO: Decide on format (.asset, .json ... .asset has preference -> look at CubeWorlds!)
                // Save at a pre-defined location in the project
                // Test loading of levels...
            }

            GUILayout.FlexibleSpace();

            EditorGUILayout.EndHorizontal();
        }

        private void DrawPlayersInfo()
        {
            // Show amount of possible players
            // Show amount of current players
            GUIStyle style = new GUIStyle();

            int maxNumberOfPlayers = GetMaxPlayerCount();

            if (maxNumberOfPlayers < 2)
            {
                style.normal.textColor = Color.red;
            }
            else
            {
                style.normal.textColor = Color.green;
            }

            EditorGUILayout.LabelField("Max Possible Players:", maxNumberOfPlayers.ToString(), style);
        }

        private void DrawLevelGrid()
        {
            if (grid == null)
            {
                return;
            }

            for (int i = 0; i < grid.FlattenedTiles.Length; i++)
            {
                Tile2D tile = grid.FlattenedTiles[i];

                Texture2D tileTexture;

                switch (tile.TileType)
                {                        
                    case TileType.HQ:
                        tileTexture = hqHexTexture;
                        break;
                    
                    case TileType.None:
                        tileTexture = null;
                        continue;
                    
                    default:
                        tileTexture = groundHexTexture;
                        break;
                }

                if (tile.Owner != null)
                {
                    GUI.color = tile.Owner.PlayerID.Color;
                }

                GUI.DrawTexture(tile.Rect, tileTexture);

                GUI.color = Color.white;
            }
        }
    }
}