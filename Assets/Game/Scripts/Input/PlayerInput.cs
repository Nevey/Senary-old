using System;
using CCore.Input;

namespace CCore.Senary.Input
{
    public class PlayerInput : MonoBehaviourSingleton<PlayerInput>
    {
        private void Awake()
        {
            MouseInput.Instance.InputDownEvent += OnMouseDown;
        }

        private void OnMouseDown(object sender, MouseInputArgs e)
        {
            // throw new NotImplementedException();
        }
    }
}