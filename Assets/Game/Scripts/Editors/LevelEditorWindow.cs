using CCore.Assets;
using CCore.Senary.Grids;
using UnityEditor;
using UnityEngine;

namespace CCore.Senary.Editors
{
    public class LevelEditorWindow : EditorWindow
    {
        private int gridWidth;

        private int gridHeight;

        private Texture groundHexTexture;

        private Texture hqHexTexture;

        private SenaryGrid grid;
        
        [MenuItem("Senary/LevelEditor")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(LevelEditorWindow));
        }

        private void Awake()
        {
            titleContent = new GUIContent("v0.0.1");

            LoadTextures();
        }

        private void OnGUI()
        {
            // TODO: Define min size after level grid drawing is functioning
            minSize = new Vector2(300f, 200f);

            DrawTopHeader();

            DrawLevelProperties();

            DrawLevelGrid();
        }

        private void LoadTextures()
        {
            groundHexTexture = AssetHelper.LoadAsset<Texture2D>("groundHex");

            hqHexTexture = AssetHelper.LoadAsset<Texture2D>("hqHex");
        }

        private void CreateGrid()
        {
            grid = new SenaryGrid(gridWidth, gridHeight);
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

            // TODO: Define at different place and more nicely
            // TODO: Anchor grid horizontally in the middle of the window
            float startX = 20f;
            float startY = 120f;

            for (int x = 0; x++ < grid.Width;)
            {
                for (int y = 0; y++ < grid.Height;)
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

                    GUI.DrawTexture(rect, groundHexTexture);
                }
            }
        }
    }
}