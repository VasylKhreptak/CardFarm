using System;
using System.Collections.Generic;
using System.Linq;
using Cards.Core;
using Cards.Data;
using Cards.Factories.Data;
using Cards.Logic.Spawn;
using Extensions;
using ProgressLogic.Core;
using ScriptableObjects.Scripts.Cards;
using ScriptableObjects.Scripts.Cards.AutomatedFactories.Recipes;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Factories.Logic
{
    public class FactoryRecipeExecutor : ProgressDependentObject
    {
        [Header("References")]
        [SerializeField] protected FactoryData _cardData;
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
            _cardData = GetComponentInParent<FactoryData>(true);
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

            SpawnRecipeResults();
            ClearRecipeResources();

            ExecuteActiveRecipe();

            TryDecreaseDurability();
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

        private void TryDecreaseDurability()
        {
            if (_cardData.IsBreakable)
            {
                _cardData.Durability.Value--;
            }
        }

        private void SpawnRecipeResults()
        {
            CardData previousCard = null;

            int cardsToSpawn = _cardData.CurrentFactoryRecipe.Value.ResultCount;

            for (int i = 0; i < cardsToSpawn; i++)
            {
                CardData spawnedCard = SpawnRecipeResult();

                if (i == cardsToSpawn - 1)
                {
                    spawnedCard.LinkTo(previousCard);
                }

                previousCard = spawnedCard;
            }
        }

        private CardData SpawnRecipeResult()
        {
            Card cardToSpawn = GetCardToSpawn();

            CardData spawnedCard = _cardSpawner.SpawnAndMove(cardToSpawn, _cardData.transform.position);

            _cardData.AutomatedFactoryCallbacks.onSpawnedRecipeResult?.Invoke(cardToSpawn);

            return spawnedCard;
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

            TryLinkCardToTop(previousCard);
        }

        private void TryLinkCardToTop(CardData card)
        {
            if (card != null && _compatibleCards.IsCompatibleByCategory(card, _cardData))
            {
                card.LinkTo(_cardData);
            }
        }
    }
}
