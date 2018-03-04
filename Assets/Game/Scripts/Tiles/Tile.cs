using System;
using CCore.Senary.Grids;
using CCore.Senary.Players;
using UnityEngine;

namespace CCore.Senary.Tiles
{
    public abstract class Tile
    {
        /// <summary>
        /// Returns grid coordinates object
        /// </summary>
        /// <returns></returns>
        public GridCoordinates GridCoordinates { get; private set; }

        public TileType TileType { get; private set; }

        public TileState TileState { get; private set; }

        /// <summary>
        /// Returns the tile owner object, this could be null if this tile was not owned by anyone
        /// </summary>
        /// <returns></returns>
        public Player Owner { get; private set; }

        public Tile(int x, int y, TileType tileType, TileState tileState)
        {
            GridCoordinates = new GridCoordinates(x, y);

            TileType = tileType;

            TileState = tileState;

            Owner = null;
        }

        public void IncrementTileType()
        {
            int enumLength = Enum.GetValues(typeof(TileType)).Length;

            int index = (int)TileType;

            if (index == enumLength - 1)
            {
                TileType = (TileType)0;
            }
            else
            {
                TileType++;
            }
        }
    }
}