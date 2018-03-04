using CCore.Senary.Grids;
using CCore.Senary.Tiles;
using UnityEngine;

namespace CCore.Senary.Configs
{
    public class LevelConfig : ScriptableObject
    {
        [HideInInspector]
        [SerializeField] private GenericGrid<Tile2D> grid;

        public GenericGrid<Tile2D> Grid { get { return grid; } }

        public void SetLevelData(GenericGrid<Tile2D> grid)
        {
            this.grid = grid;
        }
    }
}