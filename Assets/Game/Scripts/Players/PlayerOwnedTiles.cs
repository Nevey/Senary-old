using System.Collections.Generic;
using CCore.Senary.Gameplay.Grid;
using CCore.Senary.Tiles;

namespace CCore.Senary.Players
{
    public class PlayerOwnedTiles : MonoBehaviourSingleton<PlayerOwnedTiles>
    {
        public List<Tile> GetOwnedTiles(Player player)
        {
            List<Tile> ownedTiles = new List<Tile>();

            Tile[] flattenedTiles = GridController.Instance.Grid.FlattenedTiles;

            for (int i = 0; i < flattenedTiles.Length; i++)
            {
                Tile tile = flattenedTiles[i];

                if (tile.Owner == player)
                {
                    ownedTiles.Add(tile);
                }
            }

            return ownedTiles;
        }

        public int GetOwnedUnitCount(Player player)
        {
            List<Tile> ownedTiles = GetOwnedTiles(player);

            int ownedUnitCount = 0;

            for (int i = 0; i < ownedTiles.Count; i++)
            {
                ownedUnitCount += ownedTiles[i].UnitCount;
            }

            return ownedUnitCount;
        }

        public int GetOwnedHQCount(Player player)
        {
            List<Tile> ownedTiles = GetOwnedTiles(player);

            int ownedHQCount = 0;

            for (int i = 0; i < ownedTiles.Count; i++)
            {
                if (ownedTiles[i].TileType == TileType.HQ)
                {
                    ownedHQCount++;
                }
            }

            return ownedHQCount;
        }
    }
}