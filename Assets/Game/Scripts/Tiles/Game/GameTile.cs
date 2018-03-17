using CCore.Senary.Gameplay;
using CCore.Senary.Gameplay.Tiles;
using CCore.Senary.Players;
using UnityEngine;

namespace CCore.Senary.Tiles
{
    public partial class Tile
    {
        private GameObject tileGameObject;

        private int unitCount;

        public GameObject TileMesh { get { return tileGameObject; } }

        public int UnitCount { get { return unitCount; } }

        private void CreateMesh(GameObject prefab, Transform parent)
        {
            tileGameObject = GameObject.Instantiate(prefab);

            tileGameObject.transform.parent = parent;

            TileData tileData = tileGameObject.GetComponent<TileData>();

            tileData.SetData(this);
        }

        private void SetupPosition(int gridWidth, int gridHeight)
        {
            Renderer tileRenderer = tileGameObject.GetComponent<Renderer>();

            Vector3 tileSize = tileRenderer.bounds.size;

            Vector3 position = new Vector3(
                tileSize.x * gridCoordinates.X,
                0f,
                -(tileSize.z * 0.75f) * gridCoordinates.Y
            );

            position.x -= tileSize.x * (gridWidth * 0.5f);
            position.z += tileSize.z * (gridHeight * 0.5f);

            if (gridCoordinates.Y % 2 == 0)
            {
                position.x += tileSize.x * 0.5f;
            }

            tileGameObject.transform.localPosition = position;
        }

        public void SetupHQVizualizer()
        {
            TileView tileView = tileGameObject.GetComponent<TileView>();

            bool isVisible = tileType == TileType.HQ;

            tileView.SetHQTextVisible(isVisible);

            tileView.SetUnitTextVisible(false);
        }

        public void SetupTile(GameObject prefab, Transform parent, int gridWidth, int gridHeight)
        {
            if (tileType == TileType.None)
            {
                return;
            }

            CreateMesh(prefab, parent);

            SetupPosition(gridWidth, gridHeight);

            SetupHQVizualizer();
        }

        public void AddUnits(int amount, Player player)
        {
            if (Owner == player || Owner.PlayerID.ID == -1)
            {
                SetOwner(player);
            }
            else
            {
                return;
            }

            unitCount += amount;

            tileGameObject.GetComponent<TileView>().AnimateAddUnits(unitCount);
        }
    }
}