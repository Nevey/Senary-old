using System;
using UnityEngine;
using MonoBehaviour = CCore.MonoBehaviour;

namespace Game.Scripts.Gameplay.Cards
{
    public class Card : MonoBehaviour
    {
        [SerializeField] private CardConfig cardConfig;

        [SerializeField] private Renderer cardBackRenderer;

        private Camera cardsCamera;
        
        private Vector3 targetPosition;

        private Vector3 currentPosition;

        private Vector3 movementVelocity;


        private Vector3 dragOffset;

        private bool isDragging;

        public Renderer CardBackRenderer
        {
            get { return cardBackRenderer; }
        }

        public event Action<Card> StartDraggingEvent;

        public event Action<Card> StopDraggingEvent;

        private void Update()
        {
            UpdatePosition();
        }
        
        private void OnMouseDown()
        {
            Vector3 cardScreenPosition = cardsCamera.WorldToScreenPoint(transform.position);

            dragOffset = cardScreenPosition - Input.mousePosition;
            
            isDragging = true;

            if (StartDraggingEvent != null)
            {
                StartDraggingEvent(this);
            }
        }

        private void OnMouseDrag()
        {
            Vector3 dragPosition = cardsCamera.ScreenToWorldPoint(Input.mousePosition + dragOffset);
            dragPosition.z = transform.position.z;

            targetPosition = dragPosition;
        }

        private void OnMouseUp()
        {
            isDragging = false;
            
            if (StopDraggingEvent != null)
            {
                StopDraggingEvent(this);
            }
        }

        private Vector3 GetSpawnPosition()
        {
            Vector3 cardScreenPosition = cardsCamera.ScreenToWorldPoint(new Vector2(0f, 0f));

            cardScreenPosition.x -= cardBackRenderer.bounds.size.x;

            return cardScreenPosition;
        }

        private void UpdatePosition()
        {
            if (Deadzone.InReach(currentPosition, targetPosition))
            {
                return;
            }

            float moveDuration = isDragging ? cardConfig.HoldMoveDuration : cardConfig.MoveDuration;

            float maxMoveSpeed = isDragging ? cardConfig.HoldMaxMoveSpeed : cardConfig.MaxMoveSpeed;
            
            currentPosition = Vector3.SmoothDamp(currentPosition, targetPosition,
                ref movementVelocity, moveDuration, maxMoveSpeed);

            transform.position = currentPosition;
        }

        public void Initialize()
        {
            cardsCamera = GameObject.FindGameObjectWithTag("CardsCamera").GetComponent<Camera>();

            transform.position = GetSpawnPosition();

            targetPosition = currentPosition = transform.position;
        }

        public void SetTargetPosition(Vector3 position)
        {
            targetPosition = position;
        }
    }
}
