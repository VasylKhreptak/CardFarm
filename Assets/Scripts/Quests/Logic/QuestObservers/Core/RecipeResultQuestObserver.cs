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
        [SerializeField] private int _targetQuantity = 1;

        private int _currentQuantity;

        private List<CardData> _subscribedCards = new List<CardData>();

        public override void StopObserving()
        {
            base.StopObserving();

            StopObservingCards();

            _currentQuantity = 0;
        }

        protected override void OnCardAdded(CardData cardData)
        {
            StartObservingCard(cardData);
        }

        protected override void OnCardRemoved(CardData cardData)
        {
            StopObservingCard(cardData);
        }

        protected override void OnCardsCleared()
        {
            StopObservingCards();
        }

        private void StartObservingCard(CardData cardData)
        {
            cardData.Callbacks.onSpawnedRecipeResult += OnSpawnedRecipeResult;

            _subscribedCards.Add(cardData);
        }

        private void StopObservingCard(CardData cardData)
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
                _currentQuantity++;

                if (_currentQuantity >= _targetQuantity)
                {
                    MarkQuestAsCompletedByAction();
                }
            }
        }
    }
}
