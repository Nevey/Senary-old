using CCore.Senary.StateMachines.UI;
using CCore.UI;
using UnityEngine;
using UnityEngine.UI;

namespace CCore.Senary.UI
{
    public class SplashView : UIView
    {
        [SerializeField] private Button playButton;

        protected override void Setup()
        {
            UIStateMachine.Instance.GetState<SplashState>().EnterEvent += OnSplashStateEnter;

            playButton.onClick.AddListener(OnPlayClicked);
        }

        private void OnSplashStateEnter()
        {
            Show();
        }

        private void OnPlayClicked()
        {
            LocalHideCompletedEvent += OnLocalHideCompleted;

            Hide();
        }

        private void OnLocalHideCompleted()
        {
            LocalHideCompletedEvent -= OnLocalHideCompleted;

            UIStateMachine.Instance.DoTransition<GameTransition>();
        }
    }
}