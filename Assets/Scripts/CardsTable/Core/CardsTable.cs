using System.Collections.Generic;
using Cards.Core;
using Cards.Data;
using ScriptableObjects.Scripts.Cards;
using UniRx;
using UnityEngine;

namespace CardsTable.Core
{
    public class CardsTable : MonoBehaviour
    {
        [Header("Preferences")]
        [SerializeField] public CompatibleCards _compatibleCards;

        private ReactiveCollection<CardDataHolder> _cards = new ReactiveCollection<CardDataHolder>();

        public IReadOnlyReactiveCollection<CardDataHolder> Cards => _cards;

        public void AddCard(CardDataHolder cardData)
        {
            _cards.Add(cardData);
        }

        public bool RemoveCard(CardDataHolder cardData)
        {
            return _cards.Remove(cardData);
        }

        public void ClearTable()
        {
            _cards.Clear();
        }

        public bool TryGetLowestGroupCard(Card card, out CardDataHolder cardData)
        {
            foreach (CardDataHolder cardInTable in _cards)
            {
                if (cardInTable.IsLastGroupCard.Value
                    && card == cardInTable.Card.Value)
                {
                    cardData = cardInTable;
                    return true;
                }
            }

            cardData = null;
            return false;
        }

        public bool TryGetLowestRecipeFreeGroupCard(Card card, out CardDataHolder cardData)
        {
            foreach (CardDataHolder cardInTable in _cards)
            {
                if (cardInTable.IsLastGroupCard.Value
                    && card == cardInTable.Card.Value
                    && cardInTable.IsTakingPartInRecipe.Value == false)
                {
                    cardData = cardInTable;
                    return true;
                }
            }

            cardData = null;
            return false;
        }

        public bool TryGetLowestUniqRecipeFreeGroupCard(CardDataHolder cardData, out CardDataHolder foundCard)
        {
            foreach (CardDataHolder cardInTable in _cards)
            {
                if (cardInTable.IsLastGroupCard.Value
                    && cardData.Card.Value == cardInTable.Card.Value
                    && cardInTable.IsTakingPartInRecipe.Value == false
                    && cardData != cardInTable)
                {
                    foundCard = cardInTable;
                    return true;
                }
            }

            foundCard = null;
            return false;
        }

        public bool TryGetLowestGroupCardOrFirst(Card card, out CardDataHolder cardData)
        {
            if (TryGetLowestGroupCard(card, out cardData)) return true;

            foreach (CardDataHolder cardInTable in _cards)
            {
                if (cardInTable.Card.Value == card)
                {
                    cardData = cardInTable;
                    return true;
                }
            }

            cardData = null;
            return false;
        }

        public bool TryGetLowestCompatibleGroupCard(Card topCard, Card card, out CardDataHolder cardData)
        {
            if (TryGetLowestRecipeFreeGroupCard(card, out CardDataHolder lowestCard))
            {
                if (_compatibleCards.IsCompatibleWithFilters(topCard, card))
                {
                    cardData = lowestCard;
                    return true;
                }
            }

            cardData = null;
            return false;
        }

        public bool TryGetLowestUniqCompatibleGroupCard(CardDataHolder topCard, Card card, out CardDataHolder cardData)
        {
            if (TryGetLowestRecipeFreeGroupCard(card, out CardDataHolder lowestCard))
            {
                if (_compatibleCards.IsCompatibleWithFilters(topCard.Card.Value, card) &&
                    topCard != lowestCard)
                {
                    cardData = lowestCard;
                    return true;
                }
            }

            cardData = null;
            return false;
        }

        public bool TryGetLowestPrioritizedCompatibleGroupCard(Card topCard, Card[] prioritizedCards, out CardDataHolder cardData)
        {
            foreach (var prioritizedCard in prioritizedCards)
            {
                if (TryGetLowestCompatibleGroupCard(topCard, prioritizedCard, out cardData))
                {
                    return true;
                }
            }

            cardData = null;
            return false;
        }

        public bool TryGetLowestUniqPrioritizedCompatibleGroupCard(CardDataHolder topCard, Card[] prioritizedCards, out CardDataHolder cardData)
        {
            foreach (var prioritizedCard in prioritizedCards)
            {
                if (TryGetLowestUniqCompatibleGroupCard(topCard, prioritizedCard, out cardData))
                {
                    return true;
                }
            }

            cardData = null;
            return false;
        }

        public int TryGetLowestGroupCards(Card card, ref CardDataHolder[] cards)
        {
            int count = 0;

            foreach (CardDataHolder cardInTable in _cards)
            {
                if (cardInTable.IsLastGroupCard.Value)
                {
                    List<CardDataHolder> cardsGroup = cardInTable.GroupCards;

                    for (int i = cardsGroup.Count - 1; i >= 0; i--)
                    {
                        if (count >= cards.Length) return count;

                        CardDataHolder groupCard = cardsGroup[i];

                        if (groupCard.Card.Value == card)
                        {
                            cards[count] = groupCard;
                            count++;
                        }
                    }
                }
            }

            return count;
        }

        public bool TryGetFirstCard(Card card, out CardDataHolder cardData)
        {
            foreach (CardDataHolder cardInTable in _cards)
            {
                if (cardInTable.Card.Value == card)
                {
                    cardData = cardInTable;
                    return true;
                }
            }

            cardData = null;
            return false;
        }

        public bool TryGetCards(Card card, out List<CardDataHolder> cardData)
        {
            cardData = new List<CardDataHolder>();

            foreach (CardDataHolder cardInTable in _cards)
            {
                if (cardInTable.Card.Value == card)
                {
                    cardData.Add(cardInTable);
                }
            }

            return cardData.Count > 0;
        }

        public int GetCardsCount(Card card)
        {
            int count = 0;

            foreach (CardDataHolder cardInTable in _cards)
            {
                if (cardInTable.Card.Value == card)
                {
                    count++;
                }
            }

            return count;
        }
    }
}
