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
            isDragging = true;

            if (StartDraggingEvent != null)
            {
                StartDraggingEvent(this);
            }
        }

        private void OnMouseDrag()
        {
            Vector3 dragPosition =
                cardsCamera.ScreenToWorldPoint(
                    new Vector3(
                        Input.mousePosition.x,
                        Input.mousePosition.y,
                        -cardsCamera.transform.position.z - cardConfig.DragCardHeigt));
            
            dragPosition.z = -cardConfig.DragCardHeigt;

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
            if (Deadzone.InReach(transform.position, targetPosition))
            {
                return;
            }

            float moveDuration = isDragging ? cardConfig.DragMoveDuration : cardConfig.MoveDuration;

            float maxMoveSpeed = isDragging ? cardConfig.DragMaxMoveSpeed : cardConfig.MaxMoveSpeed;
            
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition,
                ref movementVelocity, moveDuration, maxMoveSpeed);
        }

        public void Initialize()
        {
            cardsCamera = GameObject.FindGameObjectWithTag("CardsCamera").GetComponent<Camera>();

            transform.position = GetSpawnPosition();

            targetPosition = transform.position;
        }

        public void SetTargetPosition(Vector3 position)
        {
            targetPosition = position;
        }
    }
}
