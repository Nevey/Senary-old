using CCore.Senary.Gameplay.Tiles;
using CCore.Senary.Players;
using UnityEngine;

namespace CCore.Senary.Tiles
{
    public partial class Tile
    {
        private GameObject tileGameObject;

        private int unitCount;

        private int initialUnitCount;

        private TileGameState tileGameState;

        private TileData tileData;

        private TileView tileView;

        private Player initialOwner;

        private TileInput tileInput;

        public GameObject TileMesh { get { return tileGameObject; } }

        public int UnitCount { get { return unitCount; } }

        public TileGameState TileGameState { get { return tileGameState; } }

        public TileData TileData { get { return tileData; } }

        public TileView TileView { get { return tileView; } }

        public TileInput TileInput { get { return tileInput; } }

        private void CreateMesh(GameObject prefab, Transform parent)
        {
            tileGameObject = Object.Instantiate(prefab);

            tileGameObject.transform.parent = parent;

            tileGameObject.name = Name;

            tileData = tileGameObject.GetComponent<TileData>();

            tileData.SetData(this);

            tileView = tileGameObject.GetComponent<TileView>();

            tileInput = tileGameObject.GetComponent<TileInput>();
        }

        private void SetupPosition(int gridWidth, int gridHeight, float offset)
        {
            Renderer tileRenderer = tileGameObject.GetComponent<Renderer>();

            Vector3 tileSize = tileRenderer.bounds.size;

            Vector3 tilePosition = new Vector3(
                (tileSize.x + offset) * gridCoordinates.X,
                0f,
                -((tileSize.z * 0.75f) + offset) * gridCoordinates.Y
            );

            tilePosition.x -= tileSize.x * (gridWidth * 0.5f);
            tilePosition.z += tileSize.z * (gridHeight * 0.5f);

            if (gridCoordinates.Y % 2 == 0)
            {
                tilePosition.x += tileSize.x * 0.5f;
            }

            tileGameObject.transform.localPosition = tilePosition;
        }

        private void SetupHQVizualizer()
        {
            bool isVisible = tileType == TileType.HQ;

            tileView.SetHQTextVisible(isVisible);

            tileView.SetUnitTextVisible(false);
        }

        public void SetupTile(GameObject prefab, Transform parent, int gridWidth, int gridHeight, float offset)
        {
            if (tileType == TileType.None)
            {
                return;
            }

            CreateMesh(prefab, parent);

            SetupPosition(gridWidth, gridHeight, offset);

            SetupHQVizualizer();

            initialOwner = owner;

            initialUnitCount = unitCount;
        }

        public void SetTileGameState(TileGameState tileGameState)
        {
            this.tileGameState = tileGameState;

            if (tileGameObject == null)
            {
                return;
            }
            
            if (tileGameState == TileGameState.NotAvailable)
            {
                tileGameObject.name = Name;
            }
            else
            {
                tileGameObject.name = Name + "-" + tileGameState.ToString();
            }
        }

        public bool AddUnits(int amount, Player player)
        {
            if (owner == player || _ownerState == TileOwnedState.Free)
            {
                // This order is very important here...
                unitCount += amount;

                SetOwner(player);

                tileView.AnimateAddUnits(unitCount);

                return true;
            }

            return false;
        }

        public void ClearUnits()
        {
            unitCount = 0;

            tileView.AnimateAddUnits(unitCount);
        }

        public void ResetTile()
        {
            ClearOwner();

            if (initialOwner != null
                && initialOwner.PlayerID.ID != Player.Dummy.PlayerID.ID)
            {
                SetOwner(initialOwner);
            }

            unitCount = initialUnitCount;

            SetTileGameState(TileGameState.NotAvailable);
        }
    }
}