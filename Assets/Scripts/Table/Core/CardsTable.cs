using System.Collections.Generic;
using Cards.Core;
using Cards.Data;
using ScriptableObjects.Scripts.Cards;
using UniRx;
using UnityEngine;

namespace Table.Core
{
    public class CardsTable : MonoBehaviour
    {
        [Header("Preferences")]
        [SerializeField] public CompatibleCards _compatibleCards;

        private ReactiveCollection<CardData> _observableCards = new ReactiveCollection<CardData>();
        
        public IReadOnlyReactiveCollection<CardData> ObservableCards => _observableCards;

        public void AddCard(CardData cardData)
        {
            _observableCards.Add(cardData);
        }

        public bool RemoveCard(CardData cardData)
        {
            return _observableCards.Remove(cardData);
        }

        public void ClearTable()
        {
            _observableCards.Clear();
        }

        public bool TryGetLowestGroupCard(Card card, out CardData cardData)
        {
            foreach (CardData cardInTable in _observableCards)
            {
                if (cardInTable.IsLowestGroupCard.Value
                    && card == cardInTable.Card.Value)
                {
                    cardData = cardInTable;
                    return true;
                }
            }

            cardData = null;
            return false;
        }

        public bool TryGetLowestGroupCardOrFirst(Card card, out CardData cardData)
        {
            if (TryGetLowestGroupCard(card, out cardData)) return true;

            foreach (CardData cardInTable in _observableCards)
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

        public bool TryGetLowestCompatibleGroupCard(Card topCard, Card card, out CardData cardData)
        {
            if (TryGetLowestGroupCard(card, out CardData lowestCard))
            {
                if (_compatibleCards.IsCompatible(topCard, card))
                {
                    cardData = lowestCard;
                    return true;
                }
            }

            cardData = null;
            return false;
        }

        public int TryGetLowestGroupCards(Card card, ref CardData[] cards)
        {
            int count = 0;

            foreach (CardData cardInTable in _observableCards)
            {
                if (cardInTable.IsLowestGroupCard.Value)
                {
                    List<CardData> cardsGroup = cardInTable.GroupCards;

                    for (int i = cardsGroup.Count - 1; i >= 0; i--)
                    {
                        if (count >= cards.Length) return count;

                        CardData groupCard = cardsGroup[i];

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

        public bool TryGetFirstCard(Card card, out CardData cardData)
        {
            foreach (CardData cardInTable in _observableCards)
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

        public bool TryGetCards(Card card, out List<CardData> cardData)
        {
            cardData = new List<CardData>();

            foreach (CardData cardInTable in _observableCards)
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

            foreach (CardData cardInTable in _observableCards)
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
