using System.Collections.Generic;
using System.Linq;
using Cards.Core;
using Cards.Data;
using Quests.Logic.QuestObservers.Core;
using ScriptableObjects.Scripts.Cards.Recipes;
using UnityEngine;

namespace Quests.Logic.QuestObservers
{
    public class QuestRestartRecipeObserver : AllCardsQuestObserver
    {
        [Header("Preferences")]
        [SerializeField] private List<Card> _targetCards = new List<Card>();

        private HashSet<CardData> _observableCards = new HashSet<CardData>();

        public override void StopObserving()
        {
            base.StopObserving();

            StopObservingCards();
        }

        protected override void OnCardAdded(CardData cardData)
        {
            if (_targetCards.Contains(cardData.Card.Value))
            {
                StartObservingCard(cardData);
            }
        }

        protected override void OnCardRemoved(CardData cardData)
        {
            if (_targetCards.Contains(cardData.Card.Value))
            {
                StopObservingCard(cardData);
            }
        }

        protected override void OnCardsCleared()
        {
            StopObservingCards();
        }

        private void StartObservingCard(CardData cardData)
        {
            StopObservingCard(cardData);

            _observableCards.Add(cardData);
            cardData.Callbacks.onRestartedRecipe += OnRestartedRecipe;
        }

        private void StopObservingCard(CardData cardData)
        {
            _observableCards.Remove(cardData);
            cardData.Callbacks.onRestartedRecipe -= OnRestartedRecipe;
        }

        private void StopObservingCards()
        {
            foreach (var observableCard in _observableCards.ToList())
            {
                StopObservingCard(observableCard);
            }
        }

        private void OnRestartedRecipe(CardRecipe recipe)
        {
            _questData.IsCompletedByAction.Value = true;
            StopObserving();
        }
    }
}
