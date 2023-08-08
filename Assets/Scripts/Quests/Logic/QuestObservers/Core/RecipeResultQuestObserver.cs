using System.Collections.Generic;
using System.Linq;
using Cards.Core;
using Cards.Data;
using UnityEngine;

namespace Quests.Logic.QuestObservers.Core
{
    public class RecipeResultQuestObserver : AllCardsQuestObserver
    {
        [Header("Preferences")]
        [SerializeField] private Card _recipeResult;

        List<CardDataHolder> _subscribedCards = new List<CardDataHolder>();

        public override void StopObserving()
        {
            base.StopObserving();

            StopObservingCards();
        }

        protected override void OnCardAdded(CardDataHolder cardData)
        {
            StartObservingCard(cardData);
        }

        protected override void OnCardRemoved(CardDataHolder cardData)
        {
            StopObservingCard(cardData);
        }

        protected override void OnCardsCleared()
        {
            StopObservingCards();
        }

        private void StartObservingCard(CardDataHolder cardData)
        {
            cardData.Callbacks.onSpawnedRecipeResult += OnSpawnedRecipeResult;

            _subscribedCards.Add(cardData);
        }

        private void StopObservingCard(CardDataHolder cardData)
        {
            cardData.Callbacks.onSpawnedRecipeResult -= OnSpawnedRecipeResult;

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
