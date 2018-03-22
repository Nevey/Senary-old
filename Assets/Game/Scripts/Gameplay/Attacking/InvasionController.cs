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

            PlayerInput.Instance.TapEvent += OnTap;
        }

        private void OnInvasionStateExit()
        {
            PlayerInput.Instance.TapEvent -= OnTap;
        }

        private void OnTap(Vector2 position)
        {
            if (defendingTile.TileInput.TapTile(position))
            {
                // remove unit from attacking tile
                // add unit to defending tile

                attackingTile.AddUnits(-1, currentPlayer);

                defendingTile.AddUnits(1, currentPlayer);
            }
        }
    }
}