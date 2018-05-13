using UnityEngine;
using MonoBehaviour = CCore.MonoBehaviour;

namespace Game.Scripts.Gameplay.Cards
{
    public class SkewOnMotion : MonoBehaviour
    {
        [SerializeField] private float rotationStrength;

        private Vector3 motionSpeed;

        private Vector2 previousPosition;

        private Vector2 currentPosition;

        private Vector3 originalRotation;

        private void Awake()
        {
            previousPosition = currentPosition = transform.position;

            originalRotation = transform.eulerAngles;
        }

        private void Update()
        {
            currentPosition = transform.position;

            motionSpeed.x = -(currentPosition.y - previousPosition.y);
            motionSpeed.y = currentPosition.x - previousPosition.x;

            previousPosition = currentPosition;

            Vector3 targetRotation = (originalRotation + motionSpeed) * rotationStrength;

            transform.rotation =
                Quaternion.Slerp(Quaternion.Euler(targetRotation), transform.rotation, 0.9f);
        }
    }
}