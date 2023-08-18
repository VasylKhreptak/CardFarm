using System.Collections.Generic;
using System.Linq;
using Cards.Core;
using Cards.Data;
using Quests.Logic.QuestObservers.Core;
using UnityEngine;

namespace Quests.Logic.QuestObservers
{
    public class QuestCardClickObserver : AllCardsQuestObserver
    {
        [Header("Preferences")]
        [SerializeField] private List<Card> _targetCards = new List<Card>();

        private HashSet<CardData> _observableCards = new HashSet<CardData>();

        public override void StopObserving()
        {
            base.StopObserving();

            StopObservingCardsClick();
        }

        protected override void OnCardAdded(CardData cardData)
        {
            if (_targetCards.Contains(cardData.Card.Value))
            {
                StartObservingCardClick(cardData);
            }
        }

        protected override void OnCardRemoved(CardData cardData)
        {
            if (_targetCards.Contains(cardData.Card.Value))
            {
                StopObservingCardClick(cardData);
            }
        }

        protected override void OnCardsCleared()
        {
            StopObservingCardsClick();
        }

        private void StartObservingCardClick(CardData cardData)
        {
            StopObservingCardClick(cardData);

            _observableCards.Add(cardData);
            cardData.Callbacks.onClicked += OnClicked;
        }

        private void StopObservingCardClick(CardData cardData)
        {
            _observableCards.Remove(cardData);
            cardData.Callbacks.onClicked -= OnClicked;
        }

        private void StopObservingCardsClick()
        {
            foreach (var observableCard in _observableCards.ToList())
            {
                StopObservingCardClick(observableCard);
            }
        }

        private void OnClicked()
        {
            _questData.IsCompletedByAction.Value = true;
            StopObserving();
        }
    }
}
