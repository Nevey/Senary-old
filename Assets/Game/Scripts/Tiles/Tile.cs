using CCore.Senary.Grids;

namespace CCore.Senary.Tiles
{
    public class Tile
    {
        public GridPosition GridPosition { get; private set; }

        public TileType TileType { get; private set; }

        public TileState TileState { get; private set; }

        public TileOwner TileOwner { get; private set; }

        public Tile(int x, int y, TileType tileType, TileState tileState)
        {
            GridPosition = new GridPosition(x, y);

            TileType = tileType;

            TileState = tileState;

            TileOwner = null;
        }
    }
}