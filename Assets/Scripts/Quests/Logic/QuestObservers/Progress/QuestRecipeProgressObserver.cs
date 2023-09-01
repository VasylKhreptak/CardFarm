using System;
using System.Collections.Generic;
using Cards.Core;
using Cards.Data;
using Cards.Factories.Data;
using Cards.Zones.SellZone.Data;
using CardsTable.Core;
using Extensions;
using ProgressLogic.Core;
using Quests.Logic.QuestObservers.Core;
using UniRx;
using UnityEngine;
using Zenject;

namespace Quests.Logic.QuestObservers.Progress
{
    public class QuestRecipeProgressObserver : QuestObserver
    {
        [Header("Preferences")]
        [SerializeField] private Card _recipeResult;
        [SerializeField] private int _targetQuantity = 1;

        private int _currentQuantity;

        private IDisposable _updateSubscription;
        private IDisposable _sellZoneSubscription;

        private bool _filledProgress;
        private bool _filledProgressPart;

        private List<ListTuple> _cardsData = new List<ListTuple>();

        private SellZoneData _foundSellZone;

        private CardsTable.Core.CardsTable _cardsTable;
        private CardSelector _cardSelector;

        [Inject]
        private void Constructor(CardsTable.Core.CardsTable cardsTable, CardSelector cardSelector)
        {
            _cardsTable = cardsTable;
            _cardSelector = cardSelector;
        }

        public override void StartObserving()
        {
            StopObserving();
            StartUpdating();
            StartObservingSellZone();
        }

        public override void StopObserving()
        {
            StopUpdating();
            ClearCardsData();
            _filledProgress = false;
            _currentQuantity = 0;
            _filledProgressPart = false;
            StopObservingSellZone();
        }

        private void StartUpdating()
        {
            _updateSubscription = Observable
                .EveryUpdate()
                .DoOnSubscribe(OnUpdate)
                .Subscribe(_ => OnUpdate());
        }

        private void StopUpdating()
        {
            _updateSubscription?.Dispose();
        }

        private void OnUpdate()
        {
            if (_filledProgress) return;

            UpdateCardsData();

            ProgressDependentObject largestProgress = null;

            foreach (var listItem in _cardsData)
            {
                if (largestProgress == null || listItem.ProgressDependentObject.Progress.Value > largestProgress.Progress.Value)
                {
                    largestProgress = listItem.ProgressDependentObject;
                }
            }

            if (largestProgress == null) return;

            float recipeProgress = largestProgress.Progress.Value;

            float currentProgress = _currentQuantity / (float)_targetQuantity + recipeProgress / _targetQuantity;

            if (_filledProgressPart) return;

            float maxPossibleCurrentProgress = _currentQuantity / (float)_targetQuantity + 1 / (float)_targetQuantity;

            if (_filledProgressPart == false && currentProgress >= maxPossibleCurrentProgress - maxPossibleCurrentProgress * 0.02)
            {
                _filledProgressPart = true;
                SetProgress(maxPossibleCurrentProgress);
                return;
            }

            if (_filledProgress == false && currentProgress >= 0.98)
            {
                SetProgress(1f);
                _filledProgress = true;
                return;
            }

            SetProgress(currentProgress);
        }

        private void UpdateCardsData()
        {
            ClearCardsData();

            foreach (var card in _cardsTable.Cards)
            {
                if (card.IsAutomatedFactory)
                {
                    FactoryData factoryData = card as FactoryData;

                    if (factoryData == null) continue;

                    if (factoryData.CurrentFactoryRecipe.Value == null) continue;

                    if (factoryData.CurrentFactoryRecipe.Value.Result.Weights.Contains(x => x.Card == _recipeResult))
                    {
                        ListTuple listTuple = new ListTuple()
                        {
                            CardData = card,
                            ProgressDependentObject = factoryData.FactoryRecipeExecutor
                        };

                        _cardsData.Add(listTuple);

                        StartObservingRecipeResult(card);
                    }
                }
                else
                {
                    if (card.CurrentRecipe.Value == null) continue;

                    if (card.CurrentRecipe.Value.Result.Weights.Contains(x => x.Card == _recipeResult))
                    {
                        ListTuple listTuple = new ListTuple()
                        {
                            CardData = card,
                            ProgressDependentObject = card.RecipeExecutor
                        };

                        _cardsData.Add(listTuple);

                        StartObservingRecipeResult(card);
                    }
                }
            }
        }

        private void SetProgress(float progress)
        {
            _questData.Progress.Value = progress;
        }

        private void ClearCardsData()
        {
            foreach (var cardData in _cardsData)
            {
                StopObservingRecipeResult(cardData.CardData);
            }

            _cardsData.Clear();
        }

        private void StartObservingRecipeResult(CardData cardData)
        {
            StopObservingRecipeResult(cardData);

            cardData.Callbacks.onSpawnedRecipeResult += OnSpawnedRecipeResult;
        }

        private void StopObservingRecipeResult(CardData cardData)
        {
            cardData.Callbacks.onSpawnedRecipeResult -= OnSpawnedRecipeResult;
        }

        private void OnSpawnedRecipeResult(CardData cardData)
        {
            if (cardData.Card.Value == _recipeResult)
            {
                _currentQuantity++;
                _filledProgressPart = false;
            }
            else
            {
                SetProgress(_currentQuantity / (float)_targetQuantity);
                StartObserving();
            }
        }

        private void StartObservingSellZone()
        {
            StopObservingSellZone();

            foreach (var kvp in _cardSelector.SelectedCardsMap)
            {
                if (kvp.Key == Card.SellZone && kvp.Value.Count > 0)
                {
                    OnFoundSellZone(kvp.Value[0] as SellZoneData);

                    return;
                }
            }

            _sellZoneSubscription = _cardSelector.SelectedCardsMap.ObserveAdd().Subscribe(x =>
            {
                if (x.Key == Card.SellZone && x.Value.Count > 0)
                {
                    OnFoundSellZone(x.Value[0] as SellZoneData);
                }
            });
        }

        private void OnFoundSellZone(SellZoneData sellZoneData)
        {
            _foundSellZone = sellZoneData;

            if (_foundSellZone == null) return;

            _foundSellZone.onSoldCard += OnSoldCard;

            _sellZoneSubscription?.Dispose();
        }

        private void StopObservingSellZone()
        {
            _sellZoneSubscription?.Dispose();

            if (_foundSellZone != null)
            {
                _foundSellZone.onSoldCard -= OnSoldCard;
                _foundSellZone = null;
            }
        }

        private void OnSoldCard(Card card)
        {
            if (card == _recipeResult && _filledProgress == false)
            {
                _currentQuantity--;
            }
        }

        private class ListTuple
        {
            public CardData CardData;
            public ProgressDependentObject ProgressDependentObject;
        }
    }
}
