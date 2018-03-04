using System;
using CCore.Senary.Grids;
using UnityEngine;

namespace CCore.Senary.Tiles
{
    [Serializable]
    public class EditorTile : Tile
    {
        /// <summary>
        /// Dimensions of the tile
        /// </summary>
        /// <returns></returns>
        public Rect Rect { get; private set; }

        /// <summary>
        /// Top left point of the tile
        /// </summary>
        /// <returns></returns>
        public Vector2 Position { get; private set; }

        /// <summary>
        /// Center point of the tile
        /// </summary>
        /// <returns></returns>
        public Vector2 CenterPosition { get; private set; }

        public EditorTile(int x, int y, TileType tileType, TileState tileState)
            : base(x, y, tileType, tileState)
        {

        }

        public void SetRect(Rect rect)
        {
            Rect = rect;

            Position = new Vector2(
                rect.x,
                rect.y
            );

            CenterPosition = new Vector2(
                rect.x + rect.width * 0.5f,
                rect.y + rect.height * 0.5f
            );
        }
    }
}