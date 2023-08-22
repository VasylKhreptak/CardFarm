using System;
using System.Collections.Generic;
using Cards.Core;
using Cards.Data;
using CardsTable.Core;
using UniRx;
using UnityEngine;

namespace CardsTable.ManualCardSelectors
{
    public class InvestigatedCardsObserver : CardTableObserver
    {
        [Header("Preferences")]
        [SerializeField] private List<Card> _blackList = new List<Card>();

        private ReactiveCollection<Card> _cards = new ReactiveCollection<Card>();
        private HashSet<Card> _cardsHashSet = new HashSet<Card>();

        public IReadOnlyReactiveCollection<Card> Cards => _cards;

        public event Action<CardData> OnInvestigatedCard;

        protected override void OnAddedCard(CardData cardData)
        {
            if (_cardsHashSet.Contains(cardData.Card.Value) == false
                && _blackList.Contains(cardData.Card.Value) == false)
            {
                _cards.Add(cardData.Card.Value);
                _cardsHashSet.Add(cardData.Card.Value);
                cardData.IsNew.Value = true;
                OnInvestigatedCard?.Invoke(cardData);
                Debug.Log("Investigated card: " + cardData.gameObject.name, cardData.gameObject);
            }
        }

        protected override void OnRemovedCard(CardData cardData)
        {

        }

        protected override void ClearCards()
        {
            _cards.Clear();
            _cardsHashSet.Clear();
        }

        public bool IsInvestigated(Card card) => _cardsHashSet.Contains(card);
    }
}
