using System.Collections.Generic;
using System.Linq;
using Cards.Core;
using Cards.Data;
using Cards.Factories.Data;
using UnityEngine;

namespace Quests.Logic.QuestObservers.Core
{
    public class FactoryResultQuestObserver : AllCardsQuestObserver
    {
        [Header("Preferences")]
        [SerializeField] private Card _recipeResult;

        List<FactoryData> _subscribedCards = new List<FactoryData>();

        public override void StopObserving()
        {
            base.StopObserving();

            StopObservingCards();
        }

        protected override void OnCardAdded(CardData cardData)
        {
            if (cardData.IsAutomatedFactory)
            {
                StartObservingCard(cardData as FactoryData);
            }
        }

        protected override void OnCardRemoved(CardData cardData)
        {
            if (cardData.IsAutomatedFactory)
            {
                StopObservingCard(cardData as FactoryData);
            }
        }

        protected override void OnCardsCleared()
        {
            StopObservingCards();
        }

        private void StartObservingCard(FactoryData cardData)
        {
            cardData.AutomatedFactoryCallbacks.onSpawnedRecipeResult += OnSpawnedRecipeResult;

            _subscribedCards.Add(cardData);
        }

        private void StopObservingCard(FactoryData cardData)
        {
            cardData.AutomatedFactoryCallbacks.onSpawnedRecipeResult -= OnSpawnedRecipeResult;

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
