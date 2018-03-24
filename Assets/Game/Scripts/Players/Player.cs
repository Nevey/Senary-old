using System;
using System.Collections.Generic;
using CCore.Senary.Tiles;
using UnityEngine;

namespace CCore.Senary.Players
{
    [Serializable]
    public class Player
    {
        [SerializeField]
        private PlayerID playerID;

        [SerializeField]
        private List<Tile> ownedTiles;

        public PlayerID PlayerID { get { return playerID; } }

        public List<Tile> OwnedTiles { get { return ownedTiles; } }

        public int OwnedUnitCount
        {
            get
            {
                int ownedUnitCount = 0;

                for (int i = 0; i < ownedTiles.Count; i++)
                {
                    ownedUnitCount += ownedTiles[i].UnitCount;
                }

                return ownedUnitCount;
            }
        }

        public static Player Dummy
        {
            get
            {
                return new Player(PlayerID.Dummy);
            }
        }

        public event Action OwnedTilesUpdatedEvent;

        public Player(PlayerID playerID)
        {
            this.playerID = playerID;
        }

        private void DispatchOwnedTilesUpdated()
        {
            if (OwnedTilesUpdatedEvent != null)
            {
                OwnedTilesUpdatedEvent();
            }
        }

        public void AddTile(Tile tile)
        {
            if (!ownedTiles.Contains(tile))
            {
                ownedTiles.Add(tile);
            }

            DispatchOwnedTilesUpdated();
        }

        public void RemoveTile(Tile tile)
        {
            if (ownedTiles.Contains(tile))
            {
                ownedTiles.Remove(tile);
            }

            DispatchOwnedTilesUpdated();
        }
    }
}
