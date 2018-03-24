using System;
using System.Collections.Generic;
using CCore.Senary.Gameplay.Turns;
using CCore.Senary.Players;
using CCore.Senary.StateMachines.Game;
using CCore.UI;
using CCore.Utilities;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace CCore.Senary.UI
{
    public class PlayerInformationView : UIView
    {
        [SerializeField] private Text defaultUnitCountText;

        private List<Text> unitCountTexts;

        private string unFormattedString;

        private Vector3 originalUnitTextScale;
        
        protected override void Setup()
        {
            GameStateMachine.Instance.GetState<AddInitialUnitsState>().EnterEvent += OnAddInitialUnitsStateEnter;

            GameStateMachine.Instance.GetState<GameOverState>().EnterEvent += OnGameOverStateEnter;

            unFormattedString = defaultUnitCountText.text;

            originalUnitTextScale = defaultUnitCountText.transform.localScale;
        }

        private void OnAddInitialUnitsStateEnter()
        {
            Show();

            List<Player> playerList = TurnController.Instance.PlayerList;

            unitCountTexts = new List<Text>();

            for (int i = 0; i < playerList.Count; i++)
            {
                Player player = playerList[i];

                player.OwnedTilesUpdatedEvent += OnOwnedTilesUpdated;

                Text unitCountText = Instantiate(
                    defaultUnitCountText,
                    defaultUnitCountText.transform.position,
                    defaultUnitCountText.transform.rotation,
                    defaultUnitCountText.transform.parent
                );

                unitCountTexts.Add(unitCountText);
            }

            defaultUnitCountText.gameObject.SetActive(false);

            SetUnitCountText();
        }

        private void OnGameOverStateEnter()
        {
            List<Player> playerList = TurnController.Instance.PlayerList;

            for (int i = 0; i < playerList.Count; i++)
            {
                Player player = playerList[i];

                player.OwnedTilesUpdatedEvent -= OnOwnedTilesUpdated;
            }

            // Could do this in the above loop...
            for (int i = 0; i < unitCountTexts.Count; i++)
            {
                Destroy(unitCountTexts[i].gameObject);
            }

            unitCountTexts.Clear();

            defaultUnitCountText.gameObject.SetActive(true);

            Hide();
        }

        private void OnOwnedTilesUpdated()
        {
            SetUnitCountText();
        }

        private void SetUnitCountText()
        {
            for (int i = 0; i < unitCountTexts.Count; i++)
            {
                Text unitCountText = unitCountTexts[i];

                Player player = TurnController.Instance.PlayerList[i];

                string playerString = String.Format(
                    "<color={0}>{1}</color>",
                    Converter.ColorToHex(player.PlayerID.Color),
                    TurnController.Instance.GetPlayerNumber(player));

                string hqCountString = String.Format(
                    "<color={0}>{1}</color>",
                    Converter.ColorToHex(player.PlayerID.Color),
                    player.OwnedHQCount);

                string unitCountString = String.Format(
                    "<color={0}>{1}</color>",
                    Converter.ColorToHex(player.PlayerID.Color),
                    player.OwnedUnitCount);

                unitCountText.text = string.Format(
                    unFormattedString,
                    playerString,
                    hqCountString,
                    unitCountString);
            }
        }
    }
}