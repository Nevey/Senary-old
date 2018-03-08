using System;
using CCore.Senary.Grids;
using CCore.Senary.Players;
using UnityEngine;

namespace CCore.Senary.Tiles
{
    [Serializable]
    public abstract class Tile
    {
        private GridCoordinates gridCoordinates;

        private TileType tileType;

        private TileState tileState;

        private Player owner;
        
        /// <summary>
        /// Returns grid coordinates object
        /// </summary>
        /// <returns></returns>
        public GridCoordinates GridCoordinates { get { return gridCoordinates; } }

        public TileType TileType { get { return tileType; } }

        public TileState TileState { get { return tileState; } }

        /// <summary>
        /// Returns the tile owner object, this could be null if this tile was not owned by anyone
        /// </summary>
        /// <returns></returns>
        public Player Owner { get { return owner; } }

        public Tile(int x, int y, TileType tileType, TileState tileState)
        {
            this.gridCoordinates = new GridCoordinates(x, y);

            this.tileType = tileType;

            this.tileState = tileState;

            owner = null;
        }

        public void IncrementTileType()
        {
            int enumLength = Enum.GetValues(typeof(TileType)).Length;

            int index = (int)TileType;

            if (index == enumLength - 1)
            {
                tileType = (TileType)0;
            }
            else
            {
                tileType++;
            }
        }

        public void SetOwner(Player owner)
        {
            this.owner = owner;

            tileState = TileState.Owned;
        }

        public void ClearOwner()
        {
            owner = null;

            tileState = TileState.Free;
        }
    }
}
