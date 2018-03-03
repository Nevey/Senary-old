using UnityEngine;

namespace CCore.Senary.Tiles
{
    /// <summary>
    /// Not to confuse with GridPosition class. This class is about
    /// position in pixels.
    /// </summary>
    public class TilePosition
    {
        public Vector2 Position { get; private set; }
        
        public void SetPosition(Vector2 position)
        {
            Position = position;
        }
    }
}