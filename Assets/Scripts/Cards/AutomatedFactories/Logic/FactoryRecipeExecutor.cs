﻿using System;
using System.Collections.Generic;
using System.Linq;
using Cards.AutomatedFactories.Data;
using Cards.Core;
using Cards.Data;
using Cards.Logic.Spawn;
using Extensions;
using ProgressLogic.Core;
using ScriptableObjects.Scripts.Cards;
using ScriptableObjects.Scripts.Cards.AutomatedFactories.Recipes;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.AutomatedFactories.Logic
{
    public class FactoryRecipeExecutor : ProgressDependentObject
    {
        [Header("References")]
        [SerializeField] private AutomatedCardFactoryData _cardData;
        [SerializeField] private CompatibleCards _compatibleCards;

        private IDisposable _currentRecipeSubscription;

        private CardSpawner _cardSpawner;

        [Inject]
        private void Constructor(CardSpawner cardSpawner)
        {
            _cardSpawner = cardSpawner;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            _cardData = GetComponentInParent<AutomatedCardFactoryData>(true);
        }

        private void OnEnable()
        {
            StartObserving();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            StopObserving();
        }

        #endregion

        private void StartObserving()
        {
            StopObserving();

            _currentRecipeSubscription = _cardData.CurrentFactoryRecipe.Subscribe(OnCurrentRecipeChanged);
        }

        private void StopObserving()
        {
            _currentRecipeSubscription?.Dispose();
        }

        protected override void OnProgressCompleted()
        {
            if (_cardData.CurrentFactoryRecipe.Value == null ||
                _cardData.CurrentFactoryRecipe.Value.Cooldown == 0) return;

            _cardData.AutomatedFactoryCallbacks.onExecutedRecipe?.Invoke(_cardData.CurrentFactoryRecipe.Value);

            SpawnRecipeResult();
            ClearRecipeResources();

            ExecuteActiveRecipe();
        }

        private void OnCurrentRecipeChanged(FactoryRecipe recipe)
        {
            if (recipe == null || recipe.Cooldown == 0)
            {
                StopProgress();
            }
            else
            {
                StartProgress(_cardData.CurrentFactoryRecipe.Value.Cooldown);
            }
        }

        private void ExecuteActiveRecipe()
        {
            OnCurrentRecipeChanged(_cardData.CurrentFactoryRecipe.Value);
        }

        protected void SpawnRecipeResult()
        {
            Card cardToSpawn = GetCardToSpawn();

            _cardSpawner.SpawnAndMove(cardToSpawn, _cardData.transform.position);

            _cardData.AutomatedFactoryCallbacks.onSpawnedRecipeResult?.Invoke(cardToSpawn);
        }

        private Card GetCardToSpawn()
        {
            return _cardData.CurrentFactoryRecipe.Value.Result.Weights.GetByWeight(x => x.Weight).Card;
        }

        private void ClearRecipeResources()
        {
            int recipeResourcesCount = _cardData.CurrentFactoryRecipe.Value.Resources.Count;

            CardData previousCard = null;

            if (_cardData.BottomCards.Count > recipeResourcesCount)
            {
                previousCard = _cardData.BottomCards[recipeResourcesCount];
            }

            List<CardData> resourcesToRemove = _cardData.BottomCards.Take(recipeResourcesCount).ToList();

            foreach (CardData resourceCard in resourcesToRemove)
            {
                resourceCard.gameObject.SetActive(false);
            }

            if (previousCard != null && _compatibleCards.IsCompatible(previousCard.Card.Value, _cardData.Card.Value))
            {
                previousCard.LinkTo(_cardData);
            }
        }
    }
}