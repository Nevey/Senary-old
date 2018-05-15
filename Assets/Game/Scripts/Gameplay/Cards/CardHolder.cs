using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MonoBehaviour = CCore.MonoBehaviour;

namespace Game.Scripts.Gameplay.Cards
{
    public class CardHolder : MonoBehaviour
    {
        private List<Card> cards;

        private void Awake()
        {
            cards = GetComponentsInChildren<Card>().ToList();
            
            for (int i = 0; i < cards.Count; i++)
            {
                Card card = cards[i];
                
                card.StartDraggingEvent += OnStartDragging;
                card.StopDraggingEvent += OnStopDragging;
                
                card.Initialize();
                
                HoldCard(i);
            }
        }

        private void OnDestroy()
        {
            for (int i = 0; i < cards.Count; i++)
            {
                Card card = cards[i];
                
                card.StartDraggingEvent -= OnStartDragging;
                card.StopDraggingEvent -= OnStopDragging;
            }
        }

        private void OnStartDragging(Card card)
        {
            if (!cards.Contains(card))
            {
                LogError("Started dragging a card which is not recognized by CardHolder");
            }
        }

        private void OnStopDragging(Card card)
        {
            if (!cards.Contains(card))
            {
                LogError("Stopped dragging a card which is not recognized by CardHolder");
            }
            
            HoldCard(card);
        }

        private void HoldCard(int index)
        {
            Card card = cards[index];
            
            float cardWidth = card.CardBackRenderer.bounds.size.x;
            
            Vector3 heldPosition = transform.position;
            heldPosition.x += cardWidth * index;
            heldPosition.x -= (cardWidth * cards.Count) / 2f;
            heldPosition.x += cardWidth / 2f;

            cards[index].SetTargetPosition(heldPosition);
        }

        private void HoldCard(Card card)
        {
            HoldCard(cards.IndexOf(card));
        }
    }
}
