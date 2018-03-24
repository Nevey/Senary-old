using System;
using CCore.Senary.StateMachines.Game;
using CCore.UI;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace CCore.Senary.UI
{
    public class DarkLayerView : UIView
    {
        [SerializeField] private Image darkLayer;

        private Color originalColor;

        private Color transparentColor;

        private Tween tween;
        
        protected override void Setup()
        {
            GameStateMachine.Instance.GetState<GameOverState>().EnterEvent += ShowDarkLayer;

            GameStateMachine.Instance.GetState<GameOverState>().ExitEvent += HideDarkLayer;

            GameStateMachine.Instance.GetState<BattleState>().EnterEvent += ShowDarkLayer;

            GameStateMachine.Instance.GetState<BattleState>().ExitEvent += HideDarkLayer;
            
            GameStateMachine.Instance.GetState<AttackerWinBattleState>().EnterEvent += ShowDarkLayer;

            GameStateMachine.Instance.GetState<DefenderWinBattleState>().EnterEvent += ShowDarkLayer;

            GameStateMachine.Instance.GetState<AttackerWinBattleState>().ExitEvent += HideDarkLayer;

            GameStateMachine.Instance.GetState<DefenderWinBattleState>().ExitEvent += HideDarkLayer;

            originalColor = darkLayer.color;

            transparentColor = darkLayer.color;
            transparentColor.a = 0f;

            darkLayer.color = transparentColor;
        }

        private void ShowDarkLayer()
        {
            Show();

            if (tween != null
                && tween.IsPlaying())
            {
                tween.Kill();
            }

            tween = darkLayer.DOColor(originalColor, 1f);
            tween.SetEase(Ease.Linear);

            tween.Play();
        }

        private void HideDarkLayer()
        {
            if (tween != null
                && tween.IsPlaying())
            {
                tween.Kill();
            }
            
            tween = darkLayer.DOColor(transparentColor, 1f);
            tween.SetEase(Ease.Linear);

            tween.OnComplete(Hide);
            
            tween.Play();
        }
    }
}