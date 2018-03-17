using System;
using CCore.Senary.Gameplay.Tiles;
using CCore.Senary.StateMachines.Game;
using CCore.Senary.Tiles;
using UnityEngine;

namespace CCore.Senary.Gameplay.Grid
{
    [RequireComponent(typeof(GridController))]
    public class GridView : MonoBehaviour
    {
        [SerializeField] private Color groundTileColor;
        
        [SerializeField] private Color hqTileColor;

        private GridController gridController;

        private MaterialPropertyBlock propertyBlock;

        private void Awake()
        {
            gridController = GetComponent<GridController>();

            propertyBlock = new MaterialPropertyBlock();

            GameStateMachine.Instance.GetState<CreateLevelState>().ExitEvent += OnCreateLevelStateExit;

#if UNITY_EDITOR
            enabled = false;
#endif
        }

        private void OnDestroy()
        {
            GameStateMachine.Instance.GetState<CreateLevelState>().ExitEvent -= OnCreateLevelStateExit;
        }

        private void OnCreateLevelStateExit()
        {
#if UNITY_EDITOR
            enabled = true;
#elif !UNITY_EDITOR
            SetColors();
#endif
            AnimateStartHQ();
        }

        private void Update()
        {
#if UNITY_EDITOR
            SetColors();
#endif
        }

        private void SetColors()
        {
            for (int i = 0; i < gridController.Grid.FlattenedTiles.Length; i++)
            {
                Tile tile = gridController.Grid.FlattenedTiles[i];

                switch (tile.TileType)
                {
                    case TileType.Ground:
                        propertyBlock.SetColor("_Color", groundTileColor);
                        break;
                    
                    case TileType.HQ:
                        propertyBlock.SetColor("_Color", hqTileColor);
                        break;
                    
                    default:
                        continue;
                }
                
                Renderer tileRenderer = tile.TileMesh.GetComponent<Renderer>();

                tileRenderer.SetPropertyBlock(propertyBlock);
            }
        }

        private void AnimateStartHQ()
        {
            int hqIndex = 0;

            for (int i = 0; i < gridController.Grid.FlattenedTiles.Length; i++)
            {
                Tile tile = gridController.Grid.FlattenedTiles[i];

                if (tile.TileType == TileType.HQ)
                {
                    tile.TileMesh.GetComponent<HQVisualizer>().AnimateStartHQ(0.25f * hqIndex);

                    hqIndex++;
                }
            }
        }
    }
}