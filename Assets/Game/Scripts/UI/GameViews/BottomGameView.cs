using System;
using CCore.Senary.Gameplay.Attacking;
using CCore.Senary.StateMachines.Game;
using CCore.Senary.StateMachines.UI;
using CCore.Senary.Tiles;
using CCore.UI;
using UnityEngine;
using UnityEngine.UI;

namespace CCore.Senary.UI
{
    public class BottomGameView : UIView
    {
        [SerializeField] private Button endTurnButton;
        
        protected override void Setup()
        {
            GameStateMachine.Instance.GetState<ManuallyEndTurnState>().EnterEvent += OnManuallyEndTurnStateEnter;
        }

        private void OnManuallyEndTurnStateEnter()
        {
            endTurnButton.onClick.AddListener(EndTurnButtonClicked);
        }

        private void EndTurnButtonClicked()
        {
            endTurnButton.onClick.RemoveListener(EndTurnButtonClicked);

            GameStateMachine.Instance.DoTransition<CheckForWinLostTransition>();
        }
    }
}