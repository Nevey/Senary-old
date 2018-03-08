using System;
using CCore.Senary.Grids;
using UnityEngine;

namespace CCore.Senary.Tiles
{
    [Serializable]
    public class EditorTile : Tile
    {
        private Rect rect;

        private Vector2 position;

        private Vector2 centerPosition;

        /// <summary>
        /// Dimensions of the tile
        /// </summary>
        /// <returns></returns>
        public Rect Rect { get { return rect; } }

        /// <summary>
        /// Top left point of the tile
        /// </summary>
        /// <returns></returns>
        public Vector2 Position { get { return position; } }

        /// <summary>
        /// Center point of the tile
        /// </summary>
        /// <returns></returns>
        public Vector2 CenterPosition { get { return centerPosition; } }

        public EditorTile(int x, int y, TileType tileType, TileState tileState)
            : base(x, y, tileType, tileState)
        {

        }

        public void SetRect(Rect rect)
        {
            this.rect = rect;

            this.position = new Vector2(
                rect.x,
                rect.y
            );

            this.centerPosition = new Vector2(
                rect.x + rect.width * 0.5f,
                rect.y + rect.height * 0.5f
            );
        }
    }
}
