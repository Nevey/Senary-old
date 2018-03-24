using System;
using CCore.Senary.Gameplay.Units;
using CCore.Senary.StateMachines.Game;
using CCore.UI;
using CCore.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace CCore.Senary.UI
{
    public class ReinforcementsView : UIView
    {
        [SerializeField] private Text reinforcementsText;

        [SerializeField] private Color textColor;

        private string unFormattedString;

        protected override void Setup()
        {
            GameStateMachine.Instance.GetState<ReceiveUnitsState>().EnterEvent += OnReceiveUnitsStateEnter;

            GameStateMachine.Instance.GetState<PlaceUnitsState>().ExitEvent += OnPlaceUnitsStateExit;

            UnitsReceiver.Instance.ReinforcementsCountUpdatedEvent += OnReinforcementsUpdated;

            unFormattedString = reinforcementsText.text;
        }

        private void OnReceiveUnitsStateEnter()
        {
            Show();
        }

        private void OnPlaceUnitsStateExit()
        {
            Hide();
        }

        private void OnReinforcementsUpdated()
        {
            UpdateReinforcementsText();
        }

        private void UpdateReinforcementsText()
        {
            string reinforcementsString = String.Format(
                "<color={0}>{1}</color>",
                Converter.ColorToHex(textColor),
                UnitsReceiver.Instance.ReinforcementsCount);

            reinforcementsText.text = string.Format(
                unFormattedString,
                reinforcementsString);
        }
    }
}