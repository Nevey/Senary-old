using System;
using CCore.Senary.Gameplay.Turns;
using CCore.UI;
using CCore.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace CCore.Senary.UI
{
    public class TopGameView : UIView
    {
        [SerializeField] private Text playerTurnText;

        private string unFormattedString;

        protected override void Setup()
        {
            TurnController.Instance.TurnStartedEvent += OnTurnStarted;

            unFormattedString = playerTurnText.text;
        }

        private void OnTurnStarted()
        {
            string playerNumberAndColorString = String.Format(
                "<color={0}>{1}</color>",
                Converter.ColorToHex(TurnController.Instance.CurrentPlayer.PlayerID.Color),
                TurnController.Instance.CurrentPlayerIndex);

            playerTurnText.text = string.Format(
                unFormattedString,
                playerNumberAndColorString);
        }
    }
}