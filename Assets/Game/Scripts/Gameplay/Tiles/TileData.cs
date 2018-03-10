using CCore.Senary.Tiles;

namespace CCore.Senary.Gameplay.Tiles
{
    public class TileData : MonoBehaviour
    {
        private Tile tile;

        public Tile Tile { get { return tile; } }

        public void SetData(Tile tile)
        {
            this.tile = tile;
        }
    }
}