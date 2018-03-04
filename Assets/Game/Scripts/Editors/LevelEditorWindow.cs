using System;
using CCore.Assets;
using CCore.Senary.Grids;
using CCore.Senary.Tiles;
using UnityEditor;
using UnityEngine;

namespace CCore.Senary.Editors
{
    public class LevelEditorWindow : EditorWindow
    {
        private EditorEvents editorEvents = new EditorEvents();

        private int gridWidth;

        private int gridHeight;

        private Texture2D groundHexTexture;

        private Texture2D hqHexTexture;

        private GenericGrid<Tile2D> grid;
        
        [MenuItem("Senary/LevelEditor")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(LevelEditorWindow));
        }

        private void Awake()
        {
            titleContent = new GUIContent("v0.0.1");

            LoadTextures();

            editorEvents.EditorMouseEvent += OnEditorMouseEvent;
        }

        private void OnDestroy()
        {
            editorEvents.EditorMouseEvent -= OnEditorMouseEvent;
        }

        private void OnGUI()
        {
            // TODO: Define min size after level grid drawing is functioning
            minSize = new Vector2(300f, 200f);

            editorEvents.OnGUI();

            DrawTopHeader();

            DrawLevelProperties();

            DrawLevelGrid();
        }

        
        private void OnEditorMouseEvent(object sender, EditorMouseEventArgs e)
        {
            if (e.editorMouseState == EditorMouseState.Down)
            {
                if (grid == null)
                {
                    return;
                }

                Tile2D tile = GetClosestTile(e.position);

                tile.IncrementTileType();

                Repaint();
            }
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
            float startY = 120f;

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

        private void DrawLevelGrid()
        {
            if (grid == null)
            {
                return;
            }

            for (int x = 0; x < grid.Width; x++)
            {
                for (int y = 0; y < grid.Height; y++)
                {
                    Tile2D tile = grid.Tiles[x, y];

                    Texture2D tileTexture;

                    switch (tile.TileType)
                    {
                        case TileType.Ground:
                            tileTexture = groundHexTexture;
                            break;
                        
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

                    GUI.DrawTexture(tile.Rect, tileTexture);
                }
            }
        }
    }
}