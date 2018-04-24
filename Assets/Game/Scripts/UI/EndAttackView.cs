using CCore.Senary.StateMachines.Game;
using CCore.UI;
using UnityEngine;
using UnityEngine.UI;

namespace CCore.Senary.UI
{
    public class EndAttackView : UIView
    {
        [SerializeField] private Button endAttackButton;
        
        protected override void Setup()
        {
            GameStateMachine.Instance.GetState<AttackState>().EnterEvent += OnAttackStateEnter;

            GameStateMachine.Instance.GetState<AttackState>().ExitEvent += OnAttackStateExit;
        }

        private void OnAttackStateEnter()
        {
            Show();

            endAttackButton.onClick.AddListener(EndTurnButtonClicked);
        }

        private void OnAttackStateExit()
        {
            Hide();
            
            endAttackButton.onClick.RemoveListener(EndTurnButtonClicked);
        }

        private void EndTurnButtonClicked()
        {
            GameStateMachine.Instance.DoTransition<ReceiveUnitsTransition>();
        }
    }
}