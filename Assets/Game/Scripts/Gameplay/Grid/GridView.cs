using System;
using CCore.Senary.Gameplay.Tiles;
using CCore.Senary.StateMachines.Game;
using CCore.Senary.Tiles;
using UnityEngine;

namespace CCore.Senary.Gameplay.Grid
{
    public class GridView : MonoBehaviour
    {
        [Header("Grid View Properties")]

        [SerializeField] private Color defaultTileColor;

        [SerializeField] private Color takeOverColor;

        [SerializeField] private Color defenderColor;

        [SerializeField] private Color attackerColor;

        [SerializeField] private Color invadingFromColor;

        [SerializeField] private Color invadingToColor;

        private MaterialPropertyBlock propertyBlock;

        private void Awake()
        {
            propertyBlock = new MaterialPropertyBlock();

            GameStateMachine.Instance.GetState<AnimateHQState>().EnterEvent += OnAnimateHQStateEnter;

#if UNITY_EDITOR
            enabled = false;
#endif
        }

        private void OnDestroy()
        {
            GameStateMachine.Instance.GetState<AnimateHQState>().EnterEvent -= OnAnimateHQStateEnter;
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
            for (int i = 0; i < GridController.Instance.Grid.FlattenedTiles.Length; i++)
            {
                Tile tile = GridController.Instance.Grid.FlattenedTiles[i];

                if (tile.TileType == TileType.None)
                {
                    continue;
                }
                
                Color targetColor = defaultTileColor;

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
                
                Renderer tileRenderer = tile.TileMesh.GetComponent<Renderer>();

                tileRenderer.SetPropertyBlock(propertyBlock);
            }
        }

        private void AnimateStartHQ()
        {
            int hqIndex = 0;

            for (int i = 0; i < GridController.Instance.Grid.FlattenedTiles.Length; i++)
            {
                Tile tile = GridController.Instance.Grid.FlattenedTiles[i];

                if (tile.TileType != TileType.HQ)
                {
                    continue;
                }
                
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