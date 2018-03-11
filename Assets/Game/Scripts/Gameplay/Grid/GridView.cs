using System;
using CCore.Senary.StateMachines.Game;
using CCore.Senary.Tiles;
using UnityEngine;

namespace CCore.Senary.Gameplay.Grid
{
    [RequireComponent(typeof(GridController))]
    public class GridView : MonoBehaviour
    {
        private GridController gridController;

        private MaterialPropertyBlock propertyBlock;

        private void Awake()
        {
            gridController = GetComponent<GridController>();

            propertyBlock = new MaterialPropertyBlock();

            GameStateMachine.Instance.GetState<CreateLevelState>().EnterEvent += OnCreateLevelStateEnter;

            // TODO: don't use update loop
            enabled = false;
        }

        private void OnCreateLevelStateEnter()
        {
            // TODO: don't use update loop
            enabled = true;
        }

        private void Update()
        {
            // TODO: Don't do this in an update loop, but on a "grid changed event"
            for (int i = 0; i < gridController.Grid.FlattenedTiles.Length; i++)
            {
                Tile tile = gridController.Grid.FlattenedTiles[i];

                if (tile.TileType == TileType.None)
                {
                    continue;
                }

                propertyBlock.SetColor("_Color", tile.Owner.PlayerID.Color);
                
                Renderer tileRenderer = tile.TileMesh.GetComponent<Renderer>();

                tileRenderer.SetPropertyBlock(propertyBlock);
            }
        }
    }
}