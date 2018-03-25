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

        private void Update()
        {
            if (unitCountTexts != null)
            {
                UpdateUnitCountText();
            }
        }
        
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
                Text unitCountText = Instantiate(
                    defaultUnitCountText,
                    defaultUnitCountText.transform.position,
                    defaultUnitCountText.transform.rotation,
                    defaultUnitCountText.transform.parent
                );

                unitCountTexts.Add(unitCountText);
            }

            defaultUnitCountText.gameObject.SetActive(false);

            UpdateUnitCountText();
        }

        private void OnGameOverStateEnter()
        {
            for (int i = 0; i < unitCountTexts.Count; i++)
            {
                Destroy(unitCountTexts[i].gameObject);
            }

            unitCountTexts.Clear();

            unitCountTexts = null;

            defaultUnitCountText.gameObject.SetActive(true);

            Hide();
        }

        private void UpdateUnitCountText()
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
                    PlayerOwnedTiles.Instance.GetOwnedHQCount(player));

                string unitCountString = String.Format(
                    "<color={0}>{1}</color>",
                    Converter.ColorToHex(player.PlayerID.Color),
                    PlayerOwnedTiles.Instance.GetOwnedUnitCount(player));

                unitCountText.text = string.Format(
                    unFormattedString,
                    playerString,
                    hqCountString,
                    unitCountString);
            }
        }
    }
}