using System;
using CCore.Senary.Grids;
using CCore.Senary.Players;
using UnityEngine;

namespace CCore.Senary.Tiles
{
    [Serializable]
    public partial class Tile
    {
        [SerializeField]
        private GridCoordinates gridCoordinates;

        [SerializeField]
        private TileType tileType;

        [SerializeField]
        private TileOwnedState tileOwnedState;

        [SerializeField]
        private Player owner;

        [SerializeField]
        private string name;
        
        /// <summary>
        /// Returns grid coordinates object
        /// </summary>
        /// <returns></returns>
        public GridCoordinates GridCoordinates { get { return gridCoordinates; } }

        public TileType TileType { get { return tileType; } }

        public TileOwnedState TileOwnedState { get { return tileOwnedState; } }

        /// <summary>
        /// Returns the tile owner object, this could be null if this tile was not owned by anyone
        /// </summary>
        /// <returns>Player</returns>
        public Player Owner { get { return owner; } }

        public string Name { get { return name; } }

        public Tile(int x, int y, TileType tileType, TileOwnedState tileOwnedState)
        {
            this.gridCoordinates = new GridCoordinates(x, y);

            this.tileType = tileType;

            this.tileOwnedState = tileOwnedState;

            this.name = String.Format("Tile-{0}-{1}", gridCoordinates.X, gridCoordinates.Y);

            ClearOwner();
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
            owner.AddTile(this);

            this.owner = owner;

            tileOwnedState = TileOwnedState.Owned;
        }

        public void ClearOwner()
        {
            owner.RemoveTile(this);
            
            owner = Player.Dummy;

            tileOwnedState = TileOwnedState.Free;
        }
    }
}
