using System;
using System.Collections.Generic;
using System.Linq;
using Cards.Core;
using Cards.Data;
using Cards.Factories.Data;
using DG.Tweening;
using Extensions;
using ProgressLogic.Core;
using Quests.Logic.QuestObservers.Core;
using ScriptableObjects.Scripts.Cards.AutomatedFactories.Recipes;
using ScriptableObjects.Scripts.Cards.Recipes;
using UniRx;
using UnityEngine;

namespace Quests.Logic.QuestObservers.Progress
{
    public class QuestRecipeProgressObserver : AllCardsQuestObserver
    {
        [Header("Preferences")]
        [SerializeField] private Card _recipeResult;

        private Dictionary<CardData, (IDisposable, IDisposable)> _cardSubscriptions = new Dictionary<CardData, (IDisposable, IDisposable)>();

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
            StopObservingCard(cardData);

            FactoryData factoryData = cardData as FactoryData;

            IDisposable subscription;

            if (factoryData != null)
            {
                subscription = factoryData.CurrentFactoryRecipe.Select(x => cardData).Subscribe(OnCardRecipeUpdated);
            }
            else
            {
                subscription = cardData.CurrentRecipe.Select(x => cardData).Subscribe(OnCardRecipeUpdated);
            }

            _cardSubscriptions.Add(cardData, (subscription, null));
        }

        private void StopObservingCard(CardData cardData)
        {
            if (_cardSubscriptions.TryGetValue(cardData, out var subscriptions))
            {
                subscriptions.Item1?.Dispose();
                subscriptions.Item2?.Dispose();
                _cardSubscriptions.Remove(cardData);
                StopObservingResultedCard(cardData);
            }
        }

        private void StopObservingCards()
        {
            foreach (var keyValuePair in _cardSubscriptions.ToList())
            {
                keyValuePair.Value.Item1?.Dispose();
                keyValuePair.Value.Item2?.Dispose();
                StopObservingResultedCard(keyValuePair.Key);
            }

            _cardSubscriptions.Clear();
            
            Debug.Log("StopObservingCards");
        }

        private void OnCardRecipeUpdated(CardData cardData)
        {
            FactoryData factoryData = cardData as FactoryData;
            CardData targetCard;

            _cardSubscriptions.TryGetValue(cardData, out var subscriptions);

            subscriptions.Item2?.Dispose();

            IDisposable progressSubscription;

            ProgressDependentObject progressDependentObject;

            if (factoryData != null)
            {
                FactoryRecipe factoryRecipe = factoryData.CurrentFactoryRecipe.Value;

                if (factoryRecipe == null) return;

                bool hasRecipeResult = factoryRecipe.Result.Weights.Contains(x => x.Card == _recipeResult);

                if (hasRecipeResult == false) return;

                progressDependentObject = factoryData.FactoryRecipeExecutor;

                targetCard = factoryData;
            }
            else
            {
                CardRecipe cardRecipe = cardData.CurrentRecipe.Value;

                if (cardRecipe == null) return;

                bool hasRecipeResult = cardRecipe.Result.Weights.Contains(x => x.Card == _recipeResult);

                if (hasRecipeResult == false) return;

                progressDependentObject = cardData.RecipeExecutor;

                targetCard = cardData;
            }

            progressSubscription = progressDependentObject.Progress.Subscribe(SetProgress);
            StartObservingResultedCard(targetCard);

            subscriptions.Item2 = progressSubscription;
        }

        private void SetProgress(float progress)
        {
            _questData.Progress.Value = progress;
            Debug.Log($"SetProgress: {progress}");
        }

        private void StartObservingResultedCard(CardData cardData)
        {
            StopObservingResultedCard(cardData);

            cardData.Callbacks.onSpawnedRecipeResult += OnSpawnedResultedCard;
        }

        private void StopObservingResultedCard(CardData cardData)
        {
            cardData.Callbacks.onSpawnedRecipeResult -= OnSpawnedResultedCard;
        }

        private void OnSpawnedResultedCard(Card card)
        {
            if (card == _recipeResult)
            {
                StopObserving();
                SetProgress(1f);
            }
        }
    }
}
