using System;
using CCore.Senary.StateMachines.Game;
using CCore.UI;
using UnityEngine;
using UnityEngine.UI;

namespace CCore.Senary.UI
{
    public class BattleResultView : UIView
    {
        [SerializeField] private Text resultText;

        protected override void Setup()
        {
            GameStateMachine.Instance.GetState<AttackerWinBattleState>().EnterEvent += OnAttackerWinBattleStateEnter;

            GameStateMachine.Instance.GetState<DefenderWinBattleState>().EnterEvent += OnDefenderWintBattleStateEnter;
        }

        private void OnAttackerWinBattleStateEnter()
        {
            
        }

        private void OnDefenderWintBattleStateEnter()
        {
            
        }
    }
}