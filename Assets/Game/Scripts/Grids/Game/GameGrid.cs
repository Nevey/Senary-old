using CCore.Senary.Tiles;

namespace CCore.Senary.Grids
{
    public class GameGrid : GenericGrid<GameTile>
    {
        public GameGrid(int width, int height) : base(width, height)
        {

        }

        public void PasteEditorGrid(EditorGrid editorGrid)
        {
            // for (int i = 0; i < editorGrid.FlattenedTiles.Length; i++)
            // {
            //     EditorTile editorTile = editorGrid.FlattenedTiles[i];
            // };
        }
    }
}