using System;
using CCore.Senary.StateMachines.Game;
using UnityEngine;

namespace CCore.Senary.Gameplay.Units
{
    public class UnitsPlacer : MonoBehaviour
    {
        [SerializeField] private UnitsReceiver unitsReceiver;

        private void Awake()
        {
            GameStateMachine.Instance.GetState<PlaceUnitsState>().EnterEvent += OnPlaceUnitsStateEnter;
        }

        private void OnPlaceUnitsStateEnter()
        {
            // Tap a tile to add a unit
            // units receiver decrement units
        }
    }
}