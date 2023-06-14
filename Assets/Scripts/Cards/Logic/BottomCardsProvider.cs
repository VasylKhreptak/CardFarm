using System.Collections.Generic;
using Cards.Data;
using UnityEngine;

namespace Cards.Logic
{
    public class BottomCardsProvider : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        public List<CardData> FindBottomCards()
        {
            List<CardData> bottomCards = new List<CardData>();

            CardData currentCardData = _cardData;

            while (currentCardData.BottomCard.Value != null)
            {
                bottomCards.Add(currentCardData.BottomCard.Value);
                currentCardData = currentCardData.BottomCard.Value;
            }

            return bottomCards;
        }
    }
}
