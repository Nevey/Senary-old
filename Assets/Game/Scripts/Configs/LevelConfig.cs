using CCore.Senary.Grids;
using CCore.Senary.Tiles;
using UnityEngine;

namespace CCore.Senary.Configs
{
    public class LevelConfig : ScriptableObject
    {
        [SerializeField, HideInInspector]
        private EditorGrid grid;

        public EditorGrid Grid { get { return grid; } }

        public void SetLevelData(EditorGrid grid)
        {
            this.grid = grid;
        }
    }
}
