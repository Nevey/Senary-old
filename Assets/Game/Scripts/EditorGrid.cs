using System;
using CCore.Senary.Grids;
using CCore.Senary.Tiles;

namespace CCore.Senary.Configs
{
    [Serializable]
    public class EditorGrid : GenericGrid<EditorTile>
    {
        public EditorGrid(int width, int height) : base(width, height)
        {
        }
    }
}
