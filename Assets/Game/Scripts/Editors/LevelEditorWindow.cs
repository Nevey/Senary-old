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

        private string levelName;

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
                levelEditorController.UpdateTileType(position, windowRect);
            }

            levelEditorController.UpdateAvailablePlayers();

            if (mouseButton == MouseButton.Right)
            {
                levelEditorController.UpdateTileOwner(position, windowRect);
            }

            if (mouseButton == MouseButton.Middle)
            {
                levelEditorController.ClearTileOwner(position, windowRect);
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
            levelName = EditorGUILayout.TextField("Level Name", levelName);

            EditorGUILayout.Space();

            gridWidth = EditorGUILayout.IntSlider("Grid Width", gridWidth, 4, 20);

            gridHeight = EditorGUILayout.IntSlider("Grid Height", gridHeight, 4, 20);

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();

            GUILayout.FlexibleSpace();

            if (GUILayout.Button("CREATE GRID"))
            {
                levelEditorController.CreateNewGrid(gridWidth, gridHeight);
            }

            if (GUILayout.Button("SAVE LEVEL"))
            {
                levelEditorController.SaveLevel(levelName);
            }

            if (GUILayout.Button("LOAD LEVEL"))
            {
                levelEditorController.LoadLevel(levelName);
            }

            GUILayout.FlexibleSpace();

            EditorGUILayout.EndHorizontal();
        }

        private void DrawPlayersInfo()
        {
            EditorGUILayout.Space();

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
                Tile tile = levelEditorController.Grid.FlattenedTiles[i];

                if (tile.Owner != null)
                {
                    GUI.color = tile.Owner.PlayerID.Color;
                }

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

                Rect rect = tile.Rect;

                // TODO: Half window width should become a "grid offset" variable
                rect.x += windowRect.width * 0.5f;

                GUI.DrawTexture(rect, tileTexture);

                GUI.color = Color.white;
            }
        }
    }
}