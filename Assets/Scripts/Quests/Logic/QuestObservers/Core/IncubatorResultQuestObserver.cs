using System.Collections.Generic;
using System.Linq;
using Cards.Core;
using Cards.Data;
using Cards.Incubators.Data;
using UnityEngine;

namespace Quests.Logic.QuestObservers.Core
{
    public class IncubatorResultQuestObserver : AllCardsQuestObserver
    {
        [Header("Preferences")]
        [SerializeField] private Card _recipeResult;

        List<IncubatorData> _subscribedCards = new List<IncubatorData>();

        public override void StopObserving()
        {
            base.StopObserving();

            StopObservingCards();
        }

        protected override void OnCardAdded(CardData cardData)
        {
            if (cardData.IsIncubator)
            {
                StartObservingCard(cardData as IncubatorData);
            }
        }

        protected override void OnCardRemoved(CardData cardData)
        {
            if (cardData.IsIncubator)
            {
                StopObservingCard(cardData as IncubatorData);
            }
        }

        protected override void OnCardsCleared()
        {
            StopObservingCards();
        }

        private void StartObservingCard(IncubatorData cardData)
        {
            cardData.IncubatorCallbacks.onSpawnedResultedCard += OnSpawnedRecipeResult;

            _subscribedCards.Add(cardData);
        }

        private void StopObservingCard(IncubatorData cardData)
        {
            cardData.IncubatorCallbacks.onSpawnedResultedCard -= OnSpawnedRecipeResult;

            _subscribedCards.Remove(cardData);
        }

        private void StopObservingCards()
        {
            foreach (var subscribedCard in _subscribedCards.ToList())
            {
                StopObservingCard(subscribedCard);
            }

            _subscribedCards.Clear();
        }

        private void OnSpawnedRecipeResult(Card card)
        {
            if (card == _recipeResult)
            {
                MarkQuestAsCompleted();
            }
        }
    }
}
