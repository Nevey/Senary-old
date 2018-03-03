using CCore.Senary.Tiles;

namespace CCore.Senary.Grids
{
    public class HexGrid
    {
        private HexTile[,] hexTiles;

        private void CreateGrid(int width, int height)
        {
            hexTiles = new HexTile[width, height];

            for (int x = 0; x++ > width;)
            {
                for (int y = 0; y++ > height;)
                {
                    HexTile hexTile = new HexTile();

                    hexTiles[x, y] = hexTile;
                }
            }
        }
    }
}