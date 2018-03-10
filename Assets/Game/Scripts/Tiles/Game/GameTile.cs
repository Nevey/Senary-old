using UnityEngine;

namespace CCore.Senary.Tiles
{
    public class GameTile : Tile
    {
        private GameObject tileMesh;

        public GameObject TileMesh { get { return tileMesh; } }

        public GameTile(int x, int y, TileType tileType, TileState tileState)
            : base(x, y, tileType, tileState)
        {

        }

        public void CreateMesh(GameObject prefab)
        {
            tileMesh = GameObject.Instantiate(prefab);
        }

        public void SetPosition(Vector3 position)
        {
            if (tileMesh == null)
            {
                Debug.LogWarningFormat("Trying to set a position on tile mesh with ID {0} but no tile mesh was created!", Name);

                return;
            }

            tileMesh.transform.localPosition = position;
        }
    }
}