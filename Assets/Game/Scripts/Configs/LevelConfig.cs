using CCore.Senary.Grids;
using CCore.Senary.Tiles;
using UnityEngine;

namespace CCore.Senary.Configs
{
    public class LevelConfig : ScriptableObject
    {
        [HideInInspector]
        [SerializeField] private GenericGrid<EditorTile> grid;

        public GenericGrid<EditorTile> Grid { get { return grid; } }

        public void SetLevelData(GenericGrid<EditorTile> grid)
        {
            this.grid = grid;
        }
    }
}