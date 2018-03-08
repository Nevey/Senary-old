using CCore.Senary.Grids;
using CCore.Senary.Tiles;
using UnityEngine;

namespace CCore.Senary.Configs
{
    public class LevelConfig : ScriptableObject
    {
        [SerializeField] private EditorGrid grid;

        [SerializeField] private int width;

        public EditorGrid Grid { get { return grid; } }

        public void SetLevelData(EditorGrid grid)
        {
            this.grid = grid;

            this.width = grid.Width;
        }
    }
}
