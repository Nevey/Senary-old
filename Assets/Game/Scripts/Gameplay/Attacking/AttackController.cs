using System;
using System.Collections.Generic;
using CCore.Senary.Gameplay.Grid;
using CCore.Senary.Gameplay.Tiles;
using CCore.Senary.Gameplay.Turns;
using CCore.Senary.Input;
using CCore.Senary.StateMachines.Game;
using CCore.Senary.Tiles;
using UnityEngine;

namespace CCore.Senary.Gameplay.Attacking
{
    public class AttackController : MonoBehaviourSingleton<AttackController>
    {
        private enum AttackStep
        {
            SelectTarget,
            SelectAttacker,
        }

        private AttackStep attackStep = new AttackStep();

        private List<Tile> defendingTiles;

        private List<Tile> attackingTiles;

        private Tile defendingTile;

        private Tile attackingTile;

        public Tile DefendingTile { get { return defendingTile; } }

        public Tile AttackingTile { get { return attackingTile; } }

        private void Awake()
        {
            GameStateMachine.Instance.GetState<AttackState>().EnterEvent += OnAttackStateEnter;

            GameStateMachine.Instance.GetState<AttackState>().ExitEvent += OnAttackStateExit;
        }

        private void OnDestroy()
        {
            GameStateMachine.Instance.GetState<AttackState>().EnterEvent -= OnAttackStateEnter;

            GameStateMachine.Instance.GetState<AttackState>().ExitEvent -= OnAttackStateExit;
        }

        private void OnAttackStateEnter()
        {
            defendingTiles = GetTargetTiles();

            PlayerInput.Instance.TapEvent += OnTap;
        }

        private void OnAttackStateExit()
        {
            PlayerInput.Instance.TapEvent -= OnTap;
        }

        private List<Tile> GetTargetTiles()
        {
            List<Tile> targetTiles = new List<Tile>();

            for (int i = 0; i < GridController.Instance.Grid.FlattenedTiles.Length; i++)
            {
                Tile tile = GridController.Instance.Grid.FlattenedTiles[i];

                if (tile.TileType == TileType.None
                    || tile.Owner != TurnController.Instance.CurrentPlayer
                    || tile.UnitCount <= 1)
                {
                    continue;
                }

                List<Tile> adjacentTiles = GridController.Instance.GetAdjacentTiles(tile);

                for (int k = 0; k < adjacentTiles.Count; k++)
                {
                    Tile adjacentTile = adjacentTiles[k];

                    if (adjacentTile.TileOwnedState == TileOwnedState.Owned)
                    {
                        if (adjacentTile.Owner != TurnController.Instance.CurrentPlayer)
                        {
                            adjacentTile.SetTileGameState(TileGameState.AvailableAsTarget);

                            if (!targetTiles.Contains(adjacentTile))
                            {
                                targetTiles.Add(adjacentTile);
                            }
                        }
                    }
                }
            }

            return targetTiles;
        }

        private List<Tile> GetAttackerTiles()
        {
            List<Tile> attackerTiles = GridController.Instance.GetAdjacentTiles(defendingTile);

            for (int i = attackerTiles.Count - 1; i >= 0; i--)
            {
                Tile tile = attackerTiles[i];

                if (tile.Owner != TurnController.Instance.CurrentPlayer
                    || tile.UnitCount <= 1)
                {
                    attackerTiles.Remove(tile);
                }
                else
                {
                    tile.SetTileGameState(TileGameState.AvailableAsAttacker);
                }
            }

            return attackerTiles;
        }

        private void OnTap(Vector2 position)
        {
            // TODO: Split this up in several game states?
            if (attackStep == AttackStep.SelectTarget)
            {
                for (int i = 0; i < defendingTiles.Count; i++)
                {
                    Tile tile = defendingTiles[i];

                    if (tile.TileInput.TapTile(position))
                    {
                        tile.SetTileGameState(TileGameState.SelectedAsTarget);

                        defendingTile = tile;

                        attackingTiles = GetAttackerTiles();

                        attackStep = AttackStep.SelectAttacker;

                        break;
                    }
                }

                // Reset all tiles
                for (int i = 0; i < defendingTiles.Count; i++)
                {
                    Tile tile = defendingTiles[i];

                    if (tile == defendingTile)
                    {
                        continue;
                    }

                    tile.SetTileGameState(TileGameState.NotAvailable);
                }
            }
            else if (attackStep == AttackStep.SelectAttacker)
            {
                for (int i = 0; i < attackingTiles.Count; i++)
                {
                    Tile tile = attackingTiles[i];

                    if (tile.TileInput.TapTile(position))
                    {
                        tile.SetTileGameState(TileGameState.SelectedAsAttacker);

                        attackingTile = tile;

                        break;
                    }
                }

                // Reset all tiles
                for (int i = 0; i < attackingTiles.Count; i++)
                {
                    Tile tile = attackingTiles[i];

                    if (tile == attackingTile)
                    {
                        continue;
                    }

                    tile.SetTileGameState(TileGameState.NotAvailable);
                }

                GameStateMachine.Instance.DoTransition<BattleTransition>();
            }
        }
    }
}