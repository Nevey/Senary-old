using CCore.Senary.Tiles;

namespace CCore.Senary.Grids
{
    public class SenaryGrid
    {
        private Tile[,] tiles;

        public Tile[,] Tiles { get { return tiles; } }

        public int Width { get; private set; }

        public int Height { get; private set; }

        public SenaryGrid(int width, int height)
        {
            tiles = new Tile[width, height];

            Width = width;

            Height = height;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Tile tile = new Tile(x, y, TileType.Ground, TileState.Free);

                    tiles[x, y] = tile;
                }
            }
        }
    }
}
