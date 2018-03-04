using System;

namespace CCore.Senary.Grids
{
    /// <summary>
    /// Contains coordinates in grid space
    /// </summary>
    [Serializable]
    public class GridCoordinates
    {
        public int x { get; private set; }
        public int y { get; private set; }

        public GridCoordinates(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}