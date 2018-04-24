using System.Collections.Generic;
using System.Linq;
using CCore.Senary.Gameplay.Grid;
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

        private AttackStep attackStep;

        private List<Tile> defendingTiles;

        private List<Tile> attackingTiles;

        public Tile DefendingTile { get; private set; }

        public Tile AttackingTile { get; private set; }

        private void Awake()
        {
            GameStateMachine.Instance.GetState<AttackState>().PostEnterEvent += OnAttackStateEnter;

            GameStateMachine.Instance.GetState<AttackState>().ExitEvent += OnAttackStateExit;
        }

        private void OnDestroy()
        {
            GameStateMachine.Instance.GetState<AttackState>().PostEnterEvent -= OnAttackStateEnter;

            GameStateMachine.Instance.GetState<AttackState>().ExitEvent -= OnAttackStateExit;
        }

        private void OnAttackStateEnter()
        {
            if (AttackingTile != null)
            {
                AttackingTile.SetTileGameState(TileGameState.NotAvailable);
            }

            if (DefendingTile != null)
            {
                DefendingTile.SetTileGameState(TileGameState.NotAvailable);
            }

            attackStep = AttackStep.SelectTarget;

            if (!IsAttackPossible())
            {
                GameStateMachine.Instance.DoTransition<ReceiveUnitsTransition>();
                
                return;
            }

            defendingTiles = MarkAndGetDefenderTiles();

            PlayerInput.Instance.TapEvent += OnTap;
        }

        private void OnAttackStateExit()
        {
            PlayerInput.Instance.TapEvent -= OnTap;
        }

        private bool IsAttackPossible()
        {
            return GridController.Instance.Grid.FlattenedTiles.Any(tile =>
                tile.Owner == TurnController.Instance.CurrentPlayer && tile.UnitCount > 1);
        }

        private List<Tile> MarkAndGetDefenderTiles()
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

                    if (adjacentTile.TileOwnedState != TileOwnedState.Owned
                        || adjacentTile.Owner == TurnController.Instance.CurrentPlayer)
                    {
                        continue;
                    }
                    
                    adjacentTile.SetTileGameState(TileGameState.AvailableAsTarget);

                    if (!targetTiles.Contains(adjacentTile))
                    {
                        targetTiles.Add(adjacentTile);
                    }
                }
            }

            return targetTiles;
        }

        private List<Tile> MarkAndGetAttackerTiles()
        {
            List<Tile> attackerTiles = GridController.Instance.GetAdjacentTiles(DefendingTile);

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

                    if (!tile.TileInput.TapTile(position))
                    {
                        continue;
                    }
                    
                    tile.SetTileGameState(TileGameState.SelectedAsTarget);

                    DefendingTile = tile;

                    attackingTiles = MarkAndGetAttackerTiles();

                    attackStep = AttackStep.SelectAttacker;

                    break;
                }

                // Reset all tiles
                for (int i = 0; i < defendingTiles.Count; i++)
                {
                    Tile tile = defendingTiles[i];

                    if (tile == DefendingTile)
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

                    if (!tile.TileInput.TapTile(position))
                    {
                        continue;
                    }
                    
                    tile.SetTileGameState(TileGameState.SelectedAsAttacker);

                    AttackingTile = tile;

                    break;
                }

                // Reset all tiles
                for (int i = 0; i < attackingTiles.Count; i++)
                {
                    Tile tile = attackingTiles[i];

                    if (tile == AttackingTile)
                    {
                        continue;
                    }

                    tile.SetTileGameState(TileGameState.NotAvailable);
                }

                GameStateMachine.Instance.DoTransition<BattleTransition>();
            }
        }

        public void ResetTileStates()
        {
            AttackingTile.SetTileGameState(TileGameState.NotAvailable);
            
            DefendingTile.SetTileGameState(TileGameState.NotAvailable);
        }
    }
}