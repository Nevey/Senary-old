using System;
using CCore.Senary.CPhysics;
using UnityEngine;

namespace CCore.Senary.Gameplay.Dice
{
    [RequireComponent(typeof(RigidbodySleepTracker))]
    public class ThrowResult : MonoBehaviour
    {
        private RigidbodySleepTracker sleepTracker;

        private void Awake()
        {
            sleepTracker = GetComponent<RigidbodySleepTracker>();

            sleepTracker.OnSleepEvent += OnRigidbodySleep;
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

            Log("Throw result is {0}", throwResult);
        }

        private bool IsVectorPointingUp(Vector3 vector3)
        {
            return vector3.normalized == new Vector3(0f, 1f, 0f);
        }
    }
}