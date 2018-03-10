using UnityEngine;

namespace CCore.Senary.Tiles
{
    public partial class Tile
    {
        private GameObject tileMesh;

        public GameObject TileMesh { get { return tileMesh; } }

        public void SetupTile(GameObject prefab, Transform parent, int gridWidth, int gridHeight)
        {
            if (tileType == TileType.None)
            {
                return;
            }

            tileMesh = GameObject.Instantiate(prefab);

            tileMesh.transform.parent = parent;

            Renderer tileRenderer = tileMesh.GetComponent<Renderer>();

            Vector3 tileSize = tileRenderer.bounds.size;

            Vector3 position = new Vector3(
                tileSize.x * gridCoordinates.X,
                0f,
                (tileSize.z * 0.75f) * gridCoordinates.Y
            );

            position.x -= tileSize.x * (gridWidth * 0.5f);
            position.x -= tileSize.x * 0.25f;
            position.y -= tileSize.y * (gridHeight * 0.5f);

            if (gridCoordinates.Y % 2 == 0)
            {
                position.x += tileSize.x * 0.5f;
            }

            tileMesh.transform.localPosition = position;
        }
    }
}