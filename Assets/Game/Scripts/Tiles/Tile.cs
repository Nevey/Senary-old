using System;
using CCore.Senary.Grids;
using UnityEngine;

namespace CCore.Senary.Tiles
{
    public class Tile
    {
        public GridPosition GridPosition { get; private set; }

        public TilePosition TilePosition { get; private set; }

        public TileType TileType { get; private set; }

        public TileState TileState { get; private set; }

        public TileOwner TileOwner { get; private set; }

        public Tile(int x, int y, TileType tileType, TileState tileState)
        {
            GridPosition = new GridPosition(x, y);

            TilePosition = new TilePosition();

            TileType = tileType;

            TileState = tileState;

            TileOwner = null;
        }

        public void SetTilePosition(Vector2 position)
        {
            TilePosition.SetPosition(position);
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