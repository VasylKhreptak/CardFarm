using System;
using System.Collections.Generic;
using System.Linq;
using Cards.Core;
using Cards.Data;
using Cards.Logic.Spawn;
using Cards.Workers.Data;
using Data;
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

        [Header("Preferences")]
        [SerializeField] private float _timeScale = 3f;

        private IDisposable _recipeSubscription;
        private IDisposable _workersSubscription;

        private int _resultsLeft;

        private CardSpawner _cardSpawner;
        private CoinsCollector _coinsCollector;
        private Camera _camera;
        private TemporaryDataStorage _temporaryDataStorage;

        [Inject]
        private void Constructor(CardSpawner cardSpawner,
            CoinsCollector coinsCollector,
            CameraProvider cameraProvider,
            TemporaryDataStorage temporaryDataStorage)
        {
            _cardSpawner = cardSpawner;
            _coinsCollector = coinsCollector;
            _camera = cameraProvider.Value;
            _temporaryDataStorage = temporaryDataStorage;
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
            StartObservingClick();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            StopObservingRecipe();
            StopObservingWorkers();
            StopObservingClick();
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
                StartProgress(recipe.Cooldown / _timeScale);
                StartObservingWorkers();
            }
        }

        protected override void OnProgressCompleted()
        {
            if (_cardData.CurrentRecipe.Value == null || _cardData.CurrentRecipe.Value.Cooldown == 0) return;

            _cardData.Callbacks.onExecutedRecipe?.Invoke(_cardData.CurrentRecipe.Value);

            SpawnRecipeResult();
            DecreaseResourcesDurability();
        }

        private void TryStartCurrentRecipe()
        {
            if (_cardData.CurrentRecipe.Value != null && _cardData.CurrentRecipe.Value.Cooldown != 0)
            {
                StartProgress(_cardData.CurrentRecipe.Value.Cooldown / _timeScale);
            }
        }

        private void SpawnRecipeResult()
        {
            Card cardToSpawn = GetCardToSpawn();

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

        public Card GetCardToSpawn()
        {
            CardRecipe currentRecipe = _cardData.CurrentRecipe.Value;

            if (currentRecipe.UsePseudoRandom)
            {
                string key = currentRecipe.GetHashCode().ToString();

                TemporaryData<bool> spawnedFirstCardData;

                _temporaryDataStorage.GetValue(key, new TemporaryData<bool>(false), out var foundData);

                spawnedFirstCardData = foundData as TemporaryData<bool>;

                if (spawnedFirstCardData.Value == false)
                {
                    spawnedFirstCardData.Value = true;
                    _temporaryDataStorage.SetValue(key, spawnedFirstCardData);

                    return currentRecipe.FirstCard;
                }
            }

            return currentRecipe.Result.Weights.GetByWeight(x => x.Weight).Card;
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
            List<CardData> resourcesToRemove = new List<CardData>();

            List<CardData> groupCards = _cardData.GroupCards;

            foreach (var recipeResource in recipeResources)
            {
                int resourceCount = groupCards.Count(x => x.Card.Value == recipeResource);

                resourcesToRemove.AddRange(groupCards.Where(x => x.Card.Value == recipeResource).Take(resourceCount).ToList());
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

        private void StartObservingClick()
        {
            _cardData.Callbacks.onClickedAnyGroupCard += OnClickedAnyGroupCard;
        }

        private void StopObservingClick()
        {
            _cardData.Callbacks.onClickedAnyGroupCard -= OnClickedAnyGroupCard;
        }

        private void OnClickedAnyGroupCard()
        {
            if (Progress.Value == 0)
            {
                TryStartCurrentRecipe();
            }
        }
    }
}
