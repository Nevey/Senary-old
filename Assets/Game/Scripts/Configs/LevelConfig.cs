using CCore.Senary.Grids;
using CCore.Senary.Tiles;
using UnityEngine;

namespace CCore.Senary.Configs
{
    public class LevelConfig : ScriptableObject
    {
        [SerializeField]
        private GenericGrid grid;

        public GenericGrid Grid { get { return grid; } }

        public void SetLevelData(GenericGrid grid)
        {
            this.grid = grid;
        }
    }
}
