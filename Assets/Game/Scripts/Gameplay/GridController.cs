using System;
using CCore.Assets;
using CCore.Senary.Configs;
using CCore.Senary.Constants;
using CCore.Senary.Grids;
using CCore.Senary.StateMachines;
using CCore.Senary.Tiles;
using UnityEngine;

namespace CCore.Senary.Gameplay
{
    public class GridController : MonoBehaviour
    {
        [SerializeField] private string levelName = "Level_1";

        [SerializeField] private GameObject tilePrefab;

        private GenericGrid<Tile> grid;

        private void Awake()
        {
            GameStateMachine.Instance.GetState<CreateLevelState>().EnterEvent += OnCreateLevelStateEnter;
        }

        private void OnDestroy()
        {
            GameStateMachine.Instance.GetState<CreateLevelState>().EnterEvent -= OnCreateLevelStateEnter;
        }

        private void OnCreateLevelStateEnter()
        {
            string assetPath = String.Format(LevelConstants.levelAssetPath, levelName);

            LevelConfig levelConfig = AssetHelper.LoadAssetAtPath<LevelConfig>(assetPath);

            grid = Convert.ChangeType(levelConfig.Grid, typeof(GenericGrid<Tile>)) as GenericGrid<Tile>;

            // grid = new GameGrid(levelConfig.Grid.Width, levelConfig.Grid.Height);

            grid.CreateTwoDimensionalGrid();

            for (int x = 0; x < grid.Width; x++)
            {
                for (int y = 0; y < grid.Height; y++)
                {
                    // Rect rect = new Rect(
                    //     groundHexTexture.width * x,
                    //     (groundHexTexture.height * .75f) * y,
                    //     groundHexTexture.width,
                    //     groundHexTexture.height
                    // );

                    // // Adding some offsets for positioning reasons
                    // rect.x -= rect.width * (grid.Width * 0.5f);
                    // rect.x -= rect.width * 0.25f;

                    // // TODO: Define this magic number more nicely
                    // rect.y += 180f;

                    // if (y % 2 == 0)
                    // {
                    //     rect.x += groundHexTexture.width * .5f;
                    // }

                    GameTile tile = (GameTile)grid.Tiles[x, y];

                    tile.CreateMesh(tilePrefab);

                    Renderer tileRenderer = tile.TileMesh.GetComponent<Renderer>();

                    Vector3 position = new Vector3(
                        tileRenderer.bounds.size.x * x,
                        0f,
                        tileRenderer.bounds.size.z * y
                    );

                    tile.SetPosition(position);
                }
            }
        }
    }
}