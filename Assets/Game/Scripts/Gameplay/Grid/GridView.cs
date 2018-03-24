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
        [Header("Grid View Properties")]

        [SerializeField] private Color groundTileColor;
        
        [SerializeField] private Color hqTileColor;

        [SerializeField] private Color takeOverColor;

        [SerializeField] private Color defenderColor;

        [SerializeField] private Color attackerColor;

        [SerializeField] private Color invadingFromColor;

        [SerializeField] private Color invadingToColor;

        private GridController gridController;

        private MaterialPropertyBlock propertyBlock;

        private void Awake()
        {
            gridController = GetComponent<GridController>();

            propertyBlock = new MaterialPropertyBlock();

            GameStateMachine.Instance.GetState<AnimateHQState>().EnterEvent += OnAnimateHQStateEnter;

#if UNITY_EDITOR
            enabled = false;
#endif
        }

        private void OnDestroy()
        {
            GameStateMachine.Instance.GetState<CreateLevelState>().EnterEvent -= OnAnimateHQStateEnter;
        }

        private void OnAnimateHQStateEnter()
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

                        Color targetColor = groundTileColor;

                        if (GameStateMachine.Instance.CurrentState is PlaceUnitsState)
                        {
                            if (tile.TileGameState == TileGameState.AvailableForTakeOver
                                || tile.TileGameState == TileGameState.AvailableForReinforcement)
                            {
                                targetColor = takeOverColor;
                            }
                        }

                        if (GameStateMachine.Instance.CurrentState is AttackState)
                        {
                            if (tile.TileGameState == TileGameState.AvailableAsTarget
                                || tile.TileGameState == TileGameState.SelectedAsTarget)
                            {
                                targetColor = defenderColor;
                            }

                            if (tile.TileGameState == TileGameState.AvailableAsAttacker
                                || tile.TileGameState == TileGameState.SelectedAsAttacker)
                            {
                                targetColor = attackerColor;
                            }
                        }

                        if (GameStateMachine.Instance.CurrentState is InvasionState)
                        {
                            if (tile.TileGameState == TileGameState.InvadingFrom)
                            {
                                targetColor = invadingFromColor;
                            }
                            
                            if (tile.TileGameState == TileGameState.InvadingTo)
                            {
                                targetColor = invadingToColor;
                            }
                        }

                        propertyBlock.SetColor("_Color", targetColor);

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
                    tile.TileMesh.GetComponent<TileView>().AnimateStartHQ(0.25f * hqIndex, () =>
                    {
                        hqIndex--;

                        if (hqIndex == 0)
                        {
                            GameStateMachine.Instance.DoTransition<AddInitialUnitsTransition>();
                        }
                    });

                    hqIndex++;
                }
            }
        }
    }
}