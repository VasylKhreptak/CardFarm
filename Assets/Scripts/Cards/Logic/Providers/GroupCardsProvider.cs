using System.Collections.Generic;
using Cards.Data;
using UnityEngine;

namespace Cards.Logic.Providers
{
    public class GroupCardsProvider : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        public List<CardData> FindGroupCards()
        {
            List<CardData> UpperCards = _cardData.UpperCardsProvider.FindUpperCards();
            List<CardData> BottomCards = _cardData.BottomCardsProvider.FindBottomCards();

            List<CardData> GroupCards = new List<CardData>(UpperCards.Count + 1 + BottomCards.Count);

            GroupCards.AddRange(UpperCards);
            GroupCards.Add(_cardData);
            GroupCards.AddRange(BottomCards);

            return GroupCards;
        }
    }
}
