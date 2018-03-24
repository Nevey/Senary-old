using System;
using CCore.Senary.Gameplay.Grid;
using CCore.Senary.Gameplay.Turns;
using CCore.Senary.StateMachines.Game;
using CCore.Senary.Tiles;
using UnityEngine;

namespace CCore.Senary.Gameplay.Units
{
    // TODO: Rename to reinforcements controller
    public class UnitsReceiver : MonoBehaviourSingleton<UnitsReceiver>
    {
        [SerializeField] private GridController gridController;

        [SerializeField] private TurnController turnController;

        private int reinforcementsCount;

        public int ReinforcementsCount { get { return reinforcementsCount; } }

        public event Action ReinforcementsCountUpdatedEvent;

        private void Awake()
        {
            GameStateMachine.Instance.GetState<ReceiveUnitsState>().EnterEvent += OnReceiveUnitsStateEnter;
        }

        private void OnReceiveUnitsStateEnter()
        {
            // Player receives 1 unit for being alive plus 1 extra
            // unit for every owned HQ
            reinforcementsCount = 1 + GetOwnedHQCount();

            Log("Received {0} reinforcement units!", reinforcementsCount);

            DispatchReinforcementsCountUpdated();

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

        private void DispatchReinforcementsCountUpdated()
        {
            if (ReinforcementsCountUpdatedEvent != null)
            {
                ReinforcementsCountUpdatedEvent();
            }
        }

        public bool DecrementNewUnitCount()
        {
            reinforcementsCount--;

            Log("Decremented reinforcement count, {0} reinforcements left...", reinforcementsCount);

            DispatchReinforcementsCountUpdated();

            if (reinforcementsCount == 0)
            {
                return true;
            }

            return false;
        }
    }
}