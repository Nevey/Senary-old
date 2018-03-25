using System;
using CCore.Senary.Tiles;
using UnityEngine;

namespace CCore.Senary.Grids
{
    [Serializable]
    public class GenericGrid : GenericGrid<Tile>
    {
        public GenericGrid(int width, int height) : base(width, height) { }

        public GenericGrid(int width, int height, Tile[] flattenedTiles) : base (width, height, flattenedTiles) { }
    }

    [Serializable]
    public class GenericGrid<T> where T : Tile
    {
        [NonSerialized]
        private T[,] tiles;

        [SerializeField]
        private T[] flattenedTiles;

        [SerializeField]
        private int width;

        [SerializeField]
        private int height;

        public T[,] Tiles { get { return tiles; } }

        public T[] FlattenedTiles { get { return flattenedTiles; } }

        public int Width { get { return width; } }

        public int Height { get { return height; } }

        public GenericGrid(int width, int height)
        {
            this.width = width;

            this.height = height;

            flattenedTiles = new T[width * height];

            int index = 0;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    T tile = (T)Activator.CreateInstance(typeof(T), x, y, TileType.Ground, TileOwnedState.Free);

                    flattenedTiles[index] = tile;

                    index++;
                }
            }
            
            CreateTwoDimensionalGrid();
        }

        public GenericGrid(int width, int height, T[] tiles)
        {
            this.width = width;

            this.height = height;

            flattenedTiles = new T[tiles.Length];

            Array.Copy(tiles, flattenedTiles, tiles.Length);
            
            CreateTwoDimensionalGrid();
        }

        /// <summary>
        /// Creates a two dimensional grid based on the flattened grid list
        /// </summary>
        public void CreateTwoDimensionalGrid()
        {
            tiles = new T[width, height];

            int index = 0;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    tiles[x, y] = flattenedTiles[index];

                    index++;
                }
            }
        }
    }
}
