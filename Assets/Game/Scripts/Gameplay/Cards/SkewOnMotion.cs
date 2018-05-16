using UnityEngine;
using MonoBehaviour = CCore.MonoBehaviour;

namespace Game.Scripts.Gameplay.Cards
{
    [RequireComponent(typeof(Card))]
    public class SkewOnMotion : MonoBehaviour
    {
        [SerializeField] private float rotationStrength;

        private Card card;

        private Vector3 motionSpeed;

        private Vector2 previousPosition;

        private Vector2 currentPosition;

        private Vector3 originalRotation;

        private bool isEnabled;

        private void Awake()
        {
            card = GetComponent<Card>();
            card.StartDraggingEvent += OnStartDragging;
            card.StopDraggingEvent += OnStopDragging;
            
            previousPosition = currentPosition = transform.position;

            originalRotation = transform.eulerAngles;
        }

        private void OnDestroy()
        {
            card.StartDraggingEvent -= OnStartDragging;
            card.StopDraggingEvent -= OnStopDragging;
        }

        private void Update()
        {
            currentPosition = transform.position;

            motionSpeed.x = -(currentPosition.y - previousPosition.y);
            motionSpeed.y = currentPosition.x - previousPosition.x;
            motionSpeed.z = previousPosition.x - currentPosition.x;

            previousPosition = currentPosition;

            Vector3 targetRotation = (originalRotation + motionSpeed) * rotationStrength;

            if (!isEnabled)
            {
                targetRotation = originalRotation;
            }

            transform.rotation =
                Quaternion.Slerp(Quaternion.Euler(targetRotation), transform.rotation, 0.9f);
        }

        private void OnStartDragging(Card obj)
        {
            isEnabled = true;
        }

        private void OnStopDragging(Card obj)
        {
            isEnabled = false;
        }
    }
}
