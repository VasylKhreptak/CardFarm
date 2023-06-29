using System.Collections.Generic;
using System.Linq;
using Cards.AutomatedFactories.Data;
using Cards.Core;
using Cards.Data;
using UnityEngine;

namespace Quests.Logic.QuestObservers.Core
{
    public class AutomatedFactoryResultQuestObserver : AllCardsQuestObserver
    {
        [Header("Preferences")]
        [SerializeField] private Card _recipeResult;

        List<AutomatedCardFactoryData> _subscribedCards = new List<AutomatedCardFactoryData>();

        public override void StopObserving()
        {
            base.StopObserving();

            StopObservingCards();
        }

        protected override void OnCardAdded(CardData cardData)
        {
            if (cardData.IsAutomatedFactory)
            {
                StartObservingCard(cardData as AutomatedCardFactoryData);
            }
        }

        protected override void OnCardRemoved(CardData cardData)
        {
            if (cardData.IsAutomatedFactory)
            {
                StopObservingCard(cardData as AutomatedCardFactoryData);
            }
        }

        protected override void OnCardsCleared()
        {
            StopObservingCards();
        }

        private void StartObservingCard(AutomatedCardFactoryData cardData)
        {
            cardData.AutomatedFactoryCallbacks.onSpawnedRecipeResult += OnSpawnedRecipeResult;

            _subscribedCards.Add(cardData);
        }

        private void StopObservingCard(AutomatedCardFactoryData cardData)
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
