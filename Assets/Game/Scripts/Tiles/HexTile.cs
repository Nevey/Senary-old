namespace CCore.Senary.Tiles
{
    public class HexTile
    {
        public TileType TileType { get; private set; }

        public TileState TileState { get; private set; }

        public TileOwner TileOwner { get; private set; }

        private HexTile[] GetSurroundingTiles()
        {
            return new HexTile[0];
        }
    }
}