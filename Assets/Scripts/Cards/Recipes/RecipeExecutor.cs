using System;
using System.Collections.Generic;
using System.Linq;
using Cards.Core;
using Cards.Data;
using Cards.Logic.Spawn;
using Cards.Workers.Data;
using Extensions;
using Graphics.UI.Particles.Coins.Logic;
using ProgressLogic.Core;
using Providers.Graphics;
using ScriptableObjects.Scripts.Cards.Recipes;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Recipes
{
    public class RecipeExecutor : ProgressDependentObject, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        private IDisposable _recipeSubscription;
        private IDisposable _workersSubscription;

        private int _resultsLeft;

        private CardSpawner _cardSpawner;
        private CoinsCollector _coinsCollector;
        private Camera _camera;

        [Inject]
        private void Constructor(CardSpawner cardSpawner, CoinsCollector coinsCollector, CameraProvider cameraProvider)
        {
            _cardSpawner = cardSpawner;
            _coinsCollector = coinsCollector;
            _camera = cameraProvider.Value;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _cardData = GetComponentInParent<CardData>(true);
        }

        private void OnEnable()
        {
            StartObservingRecipe();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            StopObservingRecipe();
            StopObservingWorkers();
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
            if (recipe == null || recipe.Cooldown == 0)
            {
                StopProgress();
                StopObservingWorkers();
            }
            else
            {
                StartProgress(recipe.Cooldown);
                StartObservingWorkers();
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

            if (cardToSpawn == Card.Coin)
            {
                Vector3 spawnPosition = RectTransformUtility.WorldToScreenPoint(_camera, _cardData.transform.position);
                _coinsCollector.Collect(1, spawnPosition, 0f);
            }
            else
            {
                _cardSpawner.SpawnAndMove(cardToSpawn, _cardData.transform.position);
            }

            _cardData.Callbacks.onSpawnedRecipeResult?.Invoke(cardToSpawn);
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

        private void StartObservingWorkers()
        {
            StopObservingWorkers();

            List<WorkerData> workers = GetWorkers();

            if (workers.Count == 0) return;

            List<IObservable<float>> workerEfficiencyObservables = workers.Select(x => x.Efficiency as IObservable<float>).ToList();

            _workersSubscription = workerEfficiencyObservables
                .Merge()
                .Subscribe(_ =>
                {
                    float efficiency = CalculateWorkersEfficiency(workers);
                    SetTimeScale(efficiency);
                });
        }

        private void StopObservingWorkers()
        {
            _workersSubscription?.Dispose();
        }

        private List<WorkerData> GetWorkers()
        {
            List<CardData> groupCards = _cardData.GroupCards;

            return groupCards.Where(x => x.IsWorker).Select(x => x as WorkerData).ToList();
        }

        private float CalculateWorkersEfficiency(List<WorkerData> workers)
        {
            return workers.Select(x => x.Efficiency.Value).Average();
        }
    }
}
