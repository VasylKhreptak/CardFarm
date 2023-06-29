using System;
using System.Collections.Generic;
using System.Linq;
using Cards.Core;
using Cards.Data;
using Cards.Logic.Spawn;
using Constraints.CardTable;
using Extensions;
using ProgressLogic.Core;
using ScriptableObjects.Scripts.Cards.Recipes;
using Table.Core;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Recipes
{
    public class RecipeExecutor : ProgressDependentObject
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        [Header("Preferences")]
        [SerializeField] private float _resultedCardMoveDuration = 0.5f;

        [Header("Spawn Preferences")]
        [SerializeField] private float _minRange = 5f;
        [SerializeField] private float _maxRange = 7f;

        private IDisposable _recipeSubscription;

        private int _resultsLeft;

        private CardsTable _cardsTable;
        private CardSpawner _cardSpawner;
        private CardsTableBounds _cardsTableBounds;

        [Inject]
        private void Constructor(CardsTable cardsTable, CardSpawner cardSpawner, CardsTableBounds cardsTableBounds)
        {
            _cardsTable = cardsTable;
            _cardSpawner = cardSpawner;
            _cardsTableBounds = cardsTableBounds;
        }

        #region MonoBehaviour

        private void OnEnable()
        {
            StartObservingRecipe();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            StopObservingRecipe();
        }

        #endregion

        private void StartObservingRecipe()
        {
            StopObservingRecipe();
            _recipeSubscription = _cardData.CurrentRecipe.Subscribe(OnRecipeUpdated);
        }

        private void StopObservingRecipe()
        {
            _recipeSubscription?.Dispose();
        }

        private void OnRecipeUpdated(CardRecipe recipe)
        {
            if (recipe == null)
            {
                StopProgress();
            }
            else
            {
                StartProgress(recipe.Cooldown);
            }
        }

        protected override void OnProgressCompleted()
        {
            if (_cardData.CurrentRecipe.Value == null || _cardData.CurrentRecipe.Value.Cooldown == 0) return;

            _cardData.Callbacks.onExecutedRecipe?.Invoke(_cardData.CurrentRecipe.Value);

            SpawnRecipeResult();
            DecreaseResourcesDurability();

            if (_cardData.CurrentRecipe.Value != null && _cardData.CurrentRecipe.Value.Cooldown != 0)
            {
                StartProgress(_cardData.CurrentRecipe.Value.Cooldown);
            }
        }

        private void SpawnRecipeResult()
        {
            Card cardToSpawn = _cardData.CurrentRecipe.Value.Result.Weights.GetByWeight(x => x.Weight).Card;

            if (_cardsTable.TryGetLowestCompatibleGroupCard(cardToSpawn, cardToSpawn, out CardData lowestGroupCard))
            {
                Vector3 position = _cardData.transform.position;
                CardData spawnedCard = _cardSpawner.Spawn(cardToSpawn, position);
                spawnedCard.LinkTo(lowestGroupCard);
            }
            else
            {
                Vector3 position = _cardsTableBounds.GetRandomPositionInRange(_cardData.Collider.bounds, _minRange, _maxRange);
                CardData spawnedCard = _cardSpawner.Spawn(cardToSpawn, _cardData.transform.position);
                spawnedCard.Animations.MoveAnimation.Play(position, _resultedCardMoveDuration);
            }
        }

        private void DecreaseResourcesDurability()
        {
            CardData firstWorker = GetFirstWorker();
            CardData lowestTargetResource = GetLastResource();
            List<CardData> resourcesToRemove = GetResourcesToRemove();

            int brokenResourcesCount = 0;

            foreach (var targetResource in resourcesToRemove)
            {
                if (targetResource.Durability.Value == 1)
                {
                    brokenResourcesCount++;
                }

                targetResource.Durability.Value--;
            }

            if (brokenResourcesCount == resourcesToRemove.Count)
            {
                firstWorker.LinkTo(lowestTargetResource);
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (_cardData == null) return;

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(_cardData.transform.position, _minRange);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_cardData.transform.position, _maxRange);
        }

        private CardData GetFirstWorker()
        {
            return _cardData.GroupCards.FirstOrDefault(x => x.IsWorker);
        }

        private CardData GetLastResource()
        {
            int recipeCardsCount = _cardData.CurrentRecipe.Value.Resources.Count + _cardData.CurrentRecipe.Value.Workers.Count;

            if (_cardData.GroupCards.Count == recipeCardsCount)
            {
                return null;
            }

            return _cardData.GroupCards[_cardData.GroupCards.Count - recipeCardsCount - 1];
        }

        private List<CardData> GetResourcesToRemove()
        {
            List<Card> recipeResources = _cardData.CurrentRecipe.Value.Resources;
            List<CardData> resourcesToRemove = new List<CardData>(recipeResources.Count);

            if (_cardData.GroupCards.TryGetResources(out List<CardData> foundResources))
            {
                resourcesToRemove = foundResources.Skip(foundResources.Count - recipeResources.Count).ToList();
            }

            return resourcesToRemove;
        }
    }
}
