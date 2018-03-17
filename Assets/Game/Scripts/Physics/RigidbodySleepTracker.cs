using System;
using UnityEngine;

namespace CCore.Senary.CPhysics
{
    [RequireComponent(typeof(Rigidbody))]
    public class RigidbodySleepTracker : MonoBehaviour
    {
        private new Rigidbody rigidbody;

        private bool isSleeping = false;

        public event Action OnAwakeEvent;

        public event Action OnSleepEvent;

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (!isSleeping && rigidbody.IsSleeping())
            {
                isSleeping = true;

                DispatchIsSleeping();
            }

            if (isSleeping && !rigidbody.IsSleeping())
            {
                isSleeping = false;

                DispatchIsSleeping();
            }
        }

        private void DispatchIsSleeping()
        {
            if (isSleeping)
            {
                if (OnSleepEvent != null)
                {
                    OnSleepEvent();
                }
            }
            else
            {
                if (OnAwakeEvent != null)
                {
                    OnAwakeEvent();
                }
            }
        }
    }
}