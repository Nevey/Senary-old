using System;
using CCore.Senary.StateMachines.UI;
using CCore.UI;

namespace CCore.Senary.UI
{
    public class SplashView : UIView
    {
        protected override void Setup()
        {
            UIStateMachine.Instance.GetState<SplashState>().EnterEvent += OnSplashStateEnter;
        }

        private void OnSplashStateEnter()
        {
            DispatchRequestShow();
        }
    }
}