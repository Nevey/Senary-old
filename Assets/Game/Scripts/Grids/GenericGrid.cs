using System;
using CCore.Senary.Tiles;

namespace CCore.Senary.Grids
{
    public class GenericGrid<T> where T : class
    {
        public T[,] Tiles { get; private set; }

        public T[] FlattenedTiles { get; private set; }

        public int Width { get; private set; }

        public int Height { get; private set; }

        public GenericGrid(int width, int height)
        {
            Tiles = new T[width, height];

            FlattenedTiles = new T[width * height];

            Width = width;

            Height = height;

            int index = 0;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    T tile = (T)Activator.CreateInstance(typeof(T), x, y, TileType.Ground, TileState.Free);

                    Tiles[x, y] = tile;

                    FlattenedTiles[index] = tile;

                    index++;
                }
            }
        }
    }
}
