namespace CCore.Senary.Grids
{
    public class GridPosition
    {
        public int x { get; private set; }
        public int y { get; private set; }

        public GridPosition(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}