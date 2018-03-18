using System;
using CCore.Senary.Gameplay.Grid;
using CCore.Senary.Gameplay.Turns;
using CCore.Senary.StateMachines.Game;
using CCore.Senary.Tiles;
using UnityEngine;

namespace CCore.Senary.Gameplay.Units
{
    public class UnitsReceiver : MonoBehaviour
    {
        [SerializeField] private GridController gridController;

        [SerializeField] private TurnController turnController;

        private int newUnitsCount;

        public int NewUnitsCount { get { return newUnitsCount; } }

        private void Awake()
        {
            GameStateMachine.Instance.GetState<ReceiveUnitsState>().EnterEvent += OnReceiveUnitsStateEnter;
        }

        private void OnReceiveUnitsStateEnter()
        {
            // Player receives 1 unit for being alive plus 1 extra
            // unit for every owned HQ
            newUnitsCount = 1 + GetOwnedHQCount();

            Log("Received {0} new units", newUnitsCount);

            GameStateMachine.Instance.DoTransition<PlaceUnitsTransition>();
        }

        private int GetOwnedHQCount()
        {
            int ownedHQCount = 0;

            for (int i = 0; i < gridController.Grid.FlattenedTiles.Length; i++)
            {
                Tile tile = gridController.Grid.FlattenedTiles[i];

                if (tile.TileType == TileType.HQ && tile.Owner == turnController.CurrentPlayer)
                {
                    ownedHQCount++;
                }
            }

            return ownedHQCount;
        }

        public bool DecrementNewUnitCount()
        {
            newUnitsCount--;

            if (newUnitsCount == 0)
            {
                return true;
            }

            return false;
        }
    }
}