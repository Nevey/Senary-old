using System;
using CCore.Assets;
using CCore.Senary.Configs;
using CCore.Senary.Constants;
using CCore.Senary.Grids;
using UnityEngine;

namespace CCore.Senary.Gameplay.Grid
{
    public class GridBuilder : MonoBehaviour
    {
        [SerializeField] private GameObject tilePrefab;
        
        public GenericGrid Build(string levelName)
        {
            string assetPath = String.Format(LevelConstants.levelAssetPath, levelName);

            LevelConfig levelConfig = AssetHelper.LoadAssetAtPath<LevelConfig>(assetPath);

            GenericGrid grid = new GenericGrid(
                levelConfig.Grid.Width,
                levelConfig.Grid.Height,
                levelConfig.Grid.FlattenedTiles
            );
            
            for (int x = 0; x < grid.Width; x++)
            {
                for (int y = 0; y < grid.Height; y++)
                {
                    grid.Tiles[x, y].SetupTile(tilePrefab, transform, grid.Width, grid.Height);
                }
            }

            return grid;
        }
    }
}