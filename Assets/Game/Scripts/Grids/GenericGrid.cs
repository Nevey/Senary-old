using System;
using CCore.Senary.Tiles;

namespace CCore.Senary.Grids
{
    public class GenericGrid<T> where T : class
    {
        private T[,] tiles;

        public T[,] Tiles { get { return tiles; } }

        public int Width { get; private set; }

        public int Height { get; private set; }

        public GenericGrid(int width, int height)
        {
            tiles = new T[width, height];

            Width = width;

            Height = height;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    T tile = (T)Activator.CreateInstance(typeof(T), x, y, TileType.Ground, TileState.Free);

                    tiles[x, y] = tile;
                }
            }
        }
    }
}
