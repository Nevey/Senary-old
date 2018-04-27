using System;
using CCore.Senary.StateMachines.Game;
using CCore.Senary.Tiles;
using DG.Tweening;
using UnityEngine;

namespace CCore.Senary.Gameplay.Tiles
{
    // TODO: Rename to tile units view, add a Tile HQ View and Tile Game State View
    [RequireComponent(typeof(TileData))]
    public class TileView : MonoBehaviour
    {
        [SerializeField] private TextMesh hqText;

        [SerializeField] private TextMesh unitText;

        [SerializeField] private SpriteRenderer hqOwnedSpriteRenderer;

        [SerializeField] private SpriteRenderer ownedSpriteRenderer;

        private TileData tileData;

        private Vector3 originalUnitTextScale;

        private void Awake()
        {
            tileData = GetComponent<TileData>();

            originalUnitTextScale = unitText.transform.localScale;
        }

        private void Update()
        {
            UpdateTileTypeSprite();
            
            UpdateTileStateSprite();
        }

        /// <summary>
        /// Updates the tile (hq) sprite and color
        /// </summary>
        private void UpdateTileTypeSprite()
        {
            TileType tileType = tileData.Tile.TileType;

            bool isOwned = tileData.Tile.TileOwnedState == TileOwnedState.Owned;

            hqOwnedSpriteRenderer.enabled = false;

            ownedSpriteRenderer.enabled = false;

            if (!isOwned)
            {
                return;
            }
            
            SpriteRenderer spriteRenderer = tileType == TileType.HQ
                ? hqOwnedSpriteRenderer
                : ownedSpriteRenderer;

            spriteRenderer.enabled = true;
                
            Color playerColor = tileData.Tile.Owner.PlayerID.Color;

            spriteRenderer.color = playerColor;
        }

        /// <summary>
        /// Updates the tile state sprites, based on current game state and tile state
        /// </summary>
        private void UpdateTileStateSprite()
        {
            TileGameState tileGameState = tileData.Tile.TileGameState;
            
            switch (tileData.Tile.TileType)
            {
                case TileType.Ground:

                    // Show ground sprite
                    if (GameStateMachine.Instance.CurrentState is PlaceUnitsState)
                    {
                        if (tileGameState == TileGameState.AvailableForTakeOver
                            || tileGameState == TileGameState.AvailableForReinforcement)
                        {
//                            targetColor = takeOverColor;
                            // TODO: Create take over sprite
                        }
                    }

                    if (GameStateMachine.Instance.CurrentState is AttackState)
                    {
                        if (tileGameState == TileGameState.AvailableAsTarget
                            || tileGameState == TileGameState.SelectedAsTarget)
                        {
//                            targetColor = defenderColor;
                            // TODO: Create defender state sprite
                        }

                        if (tileGameState == TileGameState.AvailableAsAttacker
                            || tileGameState == TileGameState.SelectedAsAttacker)
                        {
//                            targetColor = attackerColor;
                            // TODO: Create attackter state sprite
                        }
                    }

                    if (GameStateMachine.Instance.CurrentState is InvasionState)
                    {
                        if (tileGameState == TileGameState.InvadingFrom)
                        {
//                            targetColor = invadingFromColor;
                            // TODO: Create invading from state sprite
                        }
                        
                        if (tileGameState == TileGameState.InvadingTo)
                        {
//                            targetColor = invadingToColor;
                            // TODO: Create invading to state sprite
                        }
                    }

                    break;
                
                case TileType.HQ:
                    // Show hq
                    
                    break;
            }
        }

        public void SetHQTextVisible(bool visible)
        {
            hqText.gameObject.SetActive(visible);
        }

        public void SetUnitTextVisible(bool visible)
        {
            unitText.gameObject.SetActive(visible);
        }

        public void AnimateStartHQ(float delay, Action callback)
        {
            SetHQTextVisible(false);

            GameObject hqGameObject = hqText.gameObject;

            Vector3 targetScale = hqGameObject.transform.localScale;

            hqGameObject.transform.localScale *= 10f;

            Tween scaleTween = hqGameObject.transform.DOScale(targetScale, 0.5f);
            scaleTween.SetEase(Ease.OutBack);
            scaleTween.SetDelay(delay);
            scaleTween.OnStart(() =>
            {
                SetHQTextVisible(true);
            });

            scaleTween.OnComplete(() =>
            {
                scaleTween.Kill();

                scaleTween = hqGameObject.transform.DOScale(Vector3.zero, 0.5f);
                scaleTween.SetEase(Ease.InBack);
                scaleTween.SetDelay(1f);
                scaleTween.OnComplete(() =>
                {
                    callback();
                });

                scaleTween.Play();
            });

            scaleTween.Play();
        }

        public void AnimateAddUnits(int totalAmount)
        {
            SetUnitTextVisible(totalAmount > 0);

            unitText.color = tileData.Tile.Owner.PlayerID.Color;

            unitText.text = totalAmount.ToString();

            Tween scaleTweenDown = unitText.transform.DOScale(originalUnitTextScale * 0.3f, 0.1f);

            Tween scaleTweenUp = unitText.transform.DOScale(originalUnitTextScale, 0.25f);
            scaleTweenUp.SetEase(Ease.OutBack);

            scaleTweenDown.OnComplete(() =>
            {
                scaleTweenDown.Kill();

                scaleTweenUp.Play();
            });

            scaleTweenDown.Play();
        }
    }
}