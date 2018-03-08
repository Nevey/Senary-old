using System;
using CCore.Senary.Tiles;
using UnityEngine;

namespace CCore.Senary.Grids
{
    [Serializable]
    public class GenericGrid<T> where T : Tile
    {
        private T[,] tiles;

        private T[] flattenedTiles;

        private int width;

        private int height;

        public T[,] Tiles { get { return tiles; } }

        public T[] FlattenedTiles { get { return flattenedTiles; } }

        public int Width { get { return width; } }

        public int Height { get { return height; } }

        public GenericGrid(int width, int height)
        {
            tiles = new T[width, height];

            flattenedTiles = new T[width * height];

            this.width = width;

            this.height = height;

            int index = 0;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    T tile = (T)Activator.CreateInstance(typeof(T), x, y, TileType.Ground, TileState.Free);

                    tiles[x, y] = tile;

                    flattenedTiles[index] = tile;

                    index++;
                }
            }
        }
    }
}
