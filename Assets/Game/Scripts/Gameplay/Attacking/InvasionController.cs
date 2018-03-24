using System;
using CCore.Senary.Gameplay.Turns;
using CCore.Senary.Input;
using CCore.Senary.Players;
using CCore.Senary.StateMachines.Game;
using CCore.Senary.Tiles;
using UnityEngine;

namespace CCore.Senary.Gameplay.Attacking
{
    public class InvasionController : MonoBehaviourSingleton<InvasionController>
    {
        private Tile attackingTile;
        
        private Tile defendingTile;

        private Player currentPlayer;

        public event Action AllowedToEndInvasionEvent;

        private void Awake()
        {
            GameStateMachine.Instance.GetState<InvasionState>().EnterEvent += OnInvasionStateEnter;

            GameStateMachine.Instance.GetState<InvasionState>().ExitEvent += OnInvasionStateExit;
        }

        private void OnInvasionStateEnter()
        {
            attackingTile = AttackController.Instance.AttackingTile;

            defendingTile = AttackController.Instance.DefendingTile;

            currentPlayer = TurnController.Instance.CurrentPlayer;

            attackingTile.SetTileGameState(TileGameState.InvadingFrom);

            defendingTile.SetTileGameState(TileGameState.InvadingTo);

            PlayerInput.Instance.TapEvent += OnTap;
        }

        private void OnInvasionStateExit()
        {
            AttackController.Instance.ResetTileStates();
            
            PlayerInput.Instance.TapEvent -= OnTap;
        }

        private void OnTap(Vector2 position)
        {
            if (attackingTile.UnitCount > 1
                && defendingTile.TileInput.TapTile(position))
            {
                attackingTile.AddUnits(-1, currentPlayer);

                defendingTile.AddUnits(1, currentPlayer);
            }

            if (defendingTile.UnitCount > 1
                && attackingTile.TileInput.TapTile(position))
            {
                attackingTile.AddUnits(1, currentPlayer);

                defendingTile.AddUnits(-1, currentPlayer);
            }

            if (attackingTile.UnitCount >= 1
                && defendingTile.UnitCount >= 1)
            {
                if (AllowedToEndInvasionEvent != null)
                {
                    AllowedToEndInvasionEvent();
                }
            }
        }
    }
}