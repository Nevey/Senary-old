using System;
using CCore.Senary.Gameplay.Turns;
using CCore.Senary.StateMachines.Game;
using CCore.UI;
using CCore.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace CCore.Senary.UI
{
    public class PlayerTurnView : UIView
    {
        [SerializeField] private Text playerTurnText;

        private string unFormattedString;

        protected override void Setup()
        {
            GameStateMachine.Instance.GetState<SelectStartPlayerState>().EnterEvent += OnSelectStartPlayerStateEnter;

            TurnController.Instance.TurnStartedEvent += OnTurnStarted;

            unFormattedString = playerTurnText.text;
        }

        private void OnSelectStartPlayerStateEnter()
        {
            Show();
        }

        private void OnTurnStarted()
        {
            string playerNumberAndColorString = String.Format(
                "<color={0}>{1}</color>",
                Converter.ColorToHex(TurnController.Instance.CurrentPlayer.PlayerID.Color),
                TurnController.Instance.CurrentPlayerNumber);

            playerTurnText.text = string.Format(
                unFormattedString,
                playerNumberAndColorString);
        }
    }
}