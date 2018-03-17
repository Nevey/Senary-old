using System.Collections.Generic;
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

        private TileAction tileAction;

        private TileData tileData;

        private TileView tileView;

        public GameObject TileMesh { get { return tileGameObject; } }

        public int UnitCount { get { return unitCount; } }

        public TileAction TileAction { get { return tileAction; } }

        public TileData TileData { get { return tileData; } }

        public TileView TileView { get { return tileView; } }

        private void CreateMesh(GameObject prefab, Transform parent)
        {
            tileGameObject = GameObject.Instantiate(prefab);

            tileGameObject.transform.parent = parent;

            tileGameObject.name = Name;

            tileData = tileGameObject.GetComponent<TileData>();

            tileData.SetData(this);

            tileView = tileGameObject.GetComponent<TileView>();
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

        public void SetAvailability(TileAction tileAction)
        {
            this.tileAction = tileAction;

            // Do some visualization..
        }

        public bool AddUnits(int amount, Player player)
        {
            if (owner == player || tileState == TileState.Free)
            {
                SetOwner(player);
            }
            else
            {
                return false;
            }

            unitCount += amount;

            tileView.AnimateAddUnits(unitCount);

            return true;
        }
    }
}