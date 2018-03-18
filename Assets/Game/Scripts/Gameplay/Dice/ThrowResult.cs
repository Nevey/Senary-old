using System;
using CCore.Senary.CPhysics;
using UnityEngine;

namespace CCore.Senary.Gameplay.Dice
{
    public enum DieType
    {
        Defender,
        Attacker,
    }

    public class ThrowResultArgs : EventArgs
    {
        public DieType dieType;

        public int throwResult;
    }

    [RequireComponent(typeof(RigidbodySleepTracker))]
    public class ThrowResult : MonoBehaviour
    {
        [SerializeField] private DieType dieType;

        private RigidbodySleepTracker sleepTracker;

        private ThrowResultArgs throwResultArgs = new ThrowResultArgs();

        public event EventHandler<ThrowResultArgs> ThrowResultEvent;

        private void Awake()
        {
            sleepTracker = GetComponent<RigidbodySleepTracker>();

            sleepTracker.OnSleepEvent += OnRigidbodySleep;
        }

        private void OnDestroy()
        {
            sleepTracker.OnSleepEvent -= OnRigidbodySleep;
        }

        private void OnRigidbodySleep()
        {
            int throwResult = 0;

            if (IsVectorPointingUp(transform.forward))
            {
                throwResult = 5;
            }
            else if (IsVectorPointingUp(-transform.forward))
            {
                throwResult = 2;
            }
            else if (IsVectorPointingUp(transform.up))
            {
                throwResult = 4;
            }
            else if (IsVectorPointingUp(-transform.up))
            {
                throwResult = 3;
            }
            else if (IsVectorPointingUp(transform.right))
            {
                throwResult = 1;
            }
            else if (IsVectorPointingUp(-transform.right))
            {
                throwResult = 6;
            }

            Log("{0}'s throw result is {1}", dieType, throwResult);

            if (ThrowResultEvent != null)
            {
                throwResultArgs.dieType = dieType;
                throwResultArgs.throwResult = throwResult;

                ThrowResultEvent(this, throwResultArgs);
            }
        }

        private bool IsVectorPointingUp(Vector3 direction)
        {
            direction = new Vector3(
                Mathf.Round(direction.x),
                Mathf.Round(direction.y),
                Mathf.Round(direction.z)
            );
            
            return direction == Vector3.up;
        }
    }
}