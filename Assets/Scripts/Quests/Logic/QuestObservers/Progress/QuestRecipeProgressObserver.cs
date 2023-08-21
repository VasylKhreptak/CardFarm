using System;
using System.Collections.Generic;
using System.Linq;
using Cards.Core;
using Cards.Data;
using Cards.Factories.Data;
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

        private Dictionary<CardData, (IDisposable, IDisposable, ProgressDependentObject, IDisposable)> _cardsData = new();

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

            _cardsData.Add(cardData, (subscription, null, null, null));
        }

        private void StopObservingCard(CardData cardData)
        {
            if (_cardsData.TryGetValue(cardData, out var subscriptions))
            {
                subscriptions.Item1?.Dispose();
                subscriptions.Item2?.Dispose();
                _cardsData.Remove(cardData);
                StopObservingResultedCard(cardData);
            }
        }

        private void StopObservingCards()
        {
            foreach (var keyValuePair in _cardsData.ToList())
            {
                keyValuePair.Value.Item1?.Dispose();
                keyValuePair.Value.Item2?.Dispose();
                keyValuePair.Value.Item4?.Dispose();
                StopObservingResultedCard(keyValuePair.Key);
            }

            _cardsData.Clear();
        }

        private void OnCardRecipeUpdated(CardData cardData)
        {
            FactoryData factoryData = cardData as FactoryData;
            CardData targetCard;

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

            StartObservingProgress(cardData, progressDependentObject);

            StartObservingResultedCard(targetCard);
        }

        private void StartObservingProgress(CardData cardData, ProgressDependentObject progressDependentObject)
        {
            _cardsData.TryGetValue(cardData, out var cardsData);

            cardsData.Item3 = progressDependentObject;

            cardsData.Item2?.Dispose();

            IDisposable progressSubscription = progressDependentObject.Progress.Subscribe(progress =>
            {
                if (_questData.IsCompletedByAction.Value) return;

                if (progress >= 1f)
                {
                    cardsData.Item2?.Dispose();
                    SetProgress(1f);
                    return;
                }

                SetProgress(progress);
            });

            cardsData.Item2 = progressSubscription;
        }

        private void SetProgress(float progress)
        {
            _questData.Progress.Value = progress;
        }

        private void StartObservingResultedCard(CardData cardData)
        {
            _cardsData.TryGetValue(cardData, out var cardsData);
            cardsData.Item4?.Dispose();

            IDisposable subscription = Observable.FromEvent<Card>(
                    handler => cardData.Callbacks.onSpawnedRecipeResult += handler,
                    handler => cardData.Callbacks.onSpawnedRecipeResult -= handler)
                .Subscribe(card =>
                {
                    OnSpawnedResultedCard(card, cardData, cardsData.Item3);
                });

            cardsData.Item4 = subscription;
        }

        private void StopObservingResultedCard(CardData cardData)
        {
            _cardsData.TryGetValue(cardData, out var cardsData);
            cardsData.Item4?.Dispose();
        }

        private void OnSpawnedResultedCard(Card card, CardData cardData, ProgressDependentObject progressDependentObject)
        {
            if (card == _recipeResult)
            {
                StopObserving();
                SetProgress(1f);
            }
            else
            {
                SetProgress(0f);
                StartObservingProgress(cardData, progressDependentObject);
            }
        }
    }
}
