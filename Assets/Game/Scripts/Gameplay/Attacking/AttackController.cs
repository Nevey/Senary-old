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

        private List<Tile> targetTiles;

        private List<Tile> attackerTiles;

        private Tile targetTile;

        private Tile attackerTile;

        public Tile TargetTile { get { return targetTile; } }

        public Tile AttackerTile { get { return attackerTile; } }

        public event Action TargetTileSelectedEvent;
        
        public event Action AttackStartedEvent;

        public event Action AttackWonEvent;

        public event Action AttackLostEvent;

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
            targetTiles = GetTargetTiles();

            PlayerInput.Instance.TapEvent += OnTap;

            if (targetTiles.Count == 0)
            {
                GameStateMachine.Instance.DoTransition<ManuallyEndTurnTransition>();
            }
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
            List<Tile> attackerTiles = GridController.Instance.GetAdjacentTiles(targetTile);

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
                for (int i = 0; i < targetTiles.Count; i++)
                {
                    Tile tile = targetTiles[i];

                    if (tile.TileInput.TapTile(position))
                    {
                        tile.SetTileGameState(TileGameState.SelectedAsTarget);

                        targetTile = tile;

                        attackerTiles = GetAttackerTiles();

                        attackStep = AttackStep.SelectAttacker;

                        break;
                    }
                }

                // Reset all tiles
                for (int i = 0; i < targetTiles.Count; i++)
                {
                    Tile tile = targetTiles[i];

                    if (tile == targetTile)
                    {
                        continue;
                    }

                    tile.SetTileGameState(TileGameState.NotAvailable);
                }
            }
            else if (attackStep == AttackStep.SelectAttacker)
            {
                for (int i = 0; i < attackerTiles.Count; i++)
                {
                    Tile tile = attackerTiles[i];

                    if (tile.TileInput.TapTile(position))
                    {
                        tile.SetTileGameState(TileGameState.SelectedAsAttacker);

                        attackerTile = tile;

                        break;
                    }
                }

                // Reset all tiles
                for (int i = 0; i < attackerTiles.Count; i++)
                {
                    Tile tile = attackerTiles[i];

                    if (tile == attackerTile)
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