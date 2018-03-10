using UnityEngine;

namespace CCore.Senary.Tiles
{
    public partial class Tile
    {
        private GameObject tileMesh;

        public GameObject TileMesh { get { return tileMesh; } }

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