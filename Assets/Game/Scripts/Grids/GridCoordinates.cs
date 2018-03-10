using System;
using UnityEngine;

namespace CCore.Senary.Grids
{
    /// <summary>
    /// Contains coordinates in grid space
    /// </summary>
    [Serializable]
    public class GridCoordinates
    {
        [SerializeField]
        private int x;
        
        [SerializeField]
        private int y;

        public int X { get { return x; } }
        public int Y { get { return y; } }

        public GridCoordinates(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
