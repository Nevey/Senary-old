using System;
using CCore.Assets;
using CCore.Senary.Configs;
using CCore.Senary.Constants;
using CCore.Senary.Grids;
using CCore.Senary.StateMachines.Game;
using UnityEngine;

namespace CCore.Senary.Gameplay.Grid
{
    public class GridController : MonoBehaviour
    {
        // TODO: Define loading the level better
        [SerializeField] private string levelName = "Level_1";

        [SerializeField] private GameObject tilePrefab;

        private GenericGrid grid;

        public GenericGrid Grid { get { return grid; } }

        private void Awake()
        {
            GameStateMachine.Instance.GetState<CreateLevelState>().PostEnterEvent += OnCreateLevelStateEnter;
        }

        private void OnDestroy()
        {
            GameStateMachine.Instance.GetState<CreateLevelState>().PostEnterEvent -= OnCreateLevelStateEnter;
        }

        private void OnCreateLevelStateEnter()
        {
            string assetPath = String.Format(LevelConstants.levelAssetPath, levelName);

            LevelConfig levelConfig = AssetHelper.LoadAssetAtPath<LevelConfig>(assetPath);

            grid = levelConfig.Grid;

            grid.CreateTwoDimensionalGrid();

            for (int x = 0; x < grid.Width; x++)
            {
                for (int y = 0; y < grid.Height; y++)
                {
                    grid.Tiles[x, y].SetupTile(tilePrefab, transform, grid.Width, grid.Height);
                }
            }

            GameStateMachine.Instance.DoTransition<PlayerInputTransition>();
        }
    }
}