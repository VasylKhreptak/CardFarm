using System.Collections.Generic;
using Cards.Data;
using UnityEngine;

namespace Cards.Logic
{
    public class UpperCardsProvider : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        public List<CardData> FindUpperCards()
        {
            List<CardData> upperCards = new List<CardData>();

            CardData currentCardData = _cardData;

            upperCards.Add(currentCardData);

            while (currentCardData.TopCard.Value != null)
            {
                upperCards.Add(currentCardData.TopCard.Value);
                currentCardData = currentCardData.TopCard.Value;
            }

            return upperCards;
        }
    }
}
