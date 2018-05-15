using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MonoBehaviour = CCore.MonoBehaviour;

namespace Game.Scripts.Gameplay.Cards
{
    public class CardHolder : MonoBehaviour
    {
        private List<Card> cards;

        private float cardPadding;

        private void Awake()
        {
            cards = GetComponentsInChildren<Card>().ToList();

            cardPadding = cards[0].CardBackRenderer.bounds.size.x;
            
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
            Vector3 heldPosition = transform.position;
            heldPosition.x += cardPadding * index;
            heldPosition.x -= (cardPadding * cards.Count) / 2f;
            heldPosition.x += cardPadding / 2f;

            cards[index].SetTargetPosition(heldPosition);
        }

        private void HoldCard(Card card)
        {
            HoldCard(cards.IndexOf(card));
        }
    }
}
