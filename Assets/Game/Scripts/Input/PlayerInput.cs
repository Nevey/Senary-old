using System;
using CCore.Input;
using UnityEngine;

namespace CCore.Senary.Input
{
    public class PlayerInput : MonoBehaviourSingleton<PlayerInput>
    {
        public event Action<Vector2> TapEvent;

        private void Awake()
        {
            MouseInput.Instance.InputDownEvent += OnMouseDown;

            TouchInput.Instance.InputEvent += OnTouch;
        }

        private void OnMouseDown(object sender, MouseInputArgs e)
        {
            if (e.keyCode == KeyCode.Mouse0)
            {
                Tap(e.position);
            }
        }

        private void OnTouch(object sender, TouchInputArgs e)
        {
            if (e.touchPhase == TouchPhase.Began)
            {
                Tap(e.position);
            }
        }

        private void Tap(Vector2 position)
        {
            if (TapEvent != null)
            {
                Log("Tapping screen at position {0}", position);
                
                TapEvent(position);
            }
        }
    }
}