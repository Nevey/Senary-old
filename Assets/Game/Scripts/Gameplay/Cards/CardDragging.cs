using UnityEngine;
using MonoBehaviour = CCore.MonoBehaviour;

namespace Game.Scripts.Gameplay.Cards
{
    public class CardDragging : MonoBehaviour
    {
        private Camera cardsCamera;

        private Vector3 dragOffset;
        
        private void Awake()
        {
            cardsCamera = GameObject.FindGameObjectWithTag("CardsCamera").GetComponent<Camera>();
        }

        private void OnMouseDown()
        {
            Vector3 cardScreenPosition = cardsCamera.WorldToScreenPoint(transform.position);

            dragOffset = cardScreenPosition - Input.mousePosition;
        }

        private void OnMouseDrag()
        {
            Vector3 targetPosition = cardsCamera.ScreenToWorldPoint(Input.mousePosition + dragOffset);
            targetPosition.z = transform.position.z;

            transform.position = targetPosition;
        }
    }
}