using System.Collections.Generic;
using System.Linq;
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
        [SerializeField] private CompatibleCards _compatibleCards;

        private IReactiveCollection<CardData> _observableCards = new ReactiveCollection<CardData>();

        public IReadOnlyReactiveCollection<CardData> ObservableCards => _observableCards;

        public List<CardData> CardsList => _observableCards.ToList();

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
    }
}
