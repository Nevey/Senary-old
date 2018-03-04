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
        private LevelEditorController levelEditorController;

        private int gridWidth;

        private int gridHeight;
        
        [MenuItem("Senary/LevelEditor")]
        public static void ShowWindow()
        {
            ShowWindow<LevelEditorWindow>();
        }

        private void Awake()
        {
            titleContent = new GUIContent("v0.0.1");

            levelEditorController = new LevelEditorController();
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
                levelEditorController.UpdateTileType(position);
            }

            levelEditorController.UpdateAvailablePlayers();

            if (mouseButton == MouseButton.Right)
            {
                levelEditorController.UpdateTileOwner(position);
            }

            if (mouseButton == MouseButton.Middle)
            {
                levelEditorController.ClearTileOwner(position);
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
                levelEditorController.CreateGrid(gridWidth, gridHeight);
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

            int maxNumberOfPlayers = levelEditorController.GetMaxPlayerCount();

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
            if (levelEditorController.Grid == null)
            {
                return;
            }

            for (int i = 0; i < levelEditorController.Grid.FlattenedTiles.Length; i++)
            {
                Tile2D tile = levelEditorController.Grid.FlattenedTiles[i];

                Texture2D tileTexture;

                switch (tile.TileType)
                {                        
                    case TileType.HQ:
                        tileTexture = levelEditorController.HqHexTexture;
                        break;
                    
                    case TileType.None:
                        tileTexture = null;
                        continue;
                    
                    default:
                        tileTexture = levelEditorController.GroundHexTexture;
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