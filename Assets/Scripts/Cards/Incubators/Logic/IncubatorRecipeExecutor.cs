using System;
using System.Collections.Generic;
using System.Linq;
using Cards.Core;
using Cards.Data;
using Cards.Incubators.Data;
using Cards.Logic.Spawn;
using Constraints.CardTable;
using Extensions;
using ProgressLogic.Core;
using ScriptableObjects.Scripts.Cards.Incubators.Recipes;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Incubators.Logic
{
    public class IncubatorRecipeExecutor : ProgressDependentObject, IValidatable
    {
        [Header("References")]
        [SerializeField] private IncubatorData _cardData;

        [Header("Spawn Preferences")]
        [SerializeField] private float _minRange = 5f;
        [SerializeField] private float _maxRange = 7f;

        private IDisposable _currentRecipeSubscription;

        private CardSpawner _cardSpawner;
        private CardsTableBounds _cardsTableBounds;

        [Inject]
        private void Constructor(CardSpawner cardSpawner, CardsTableBounds cardsTableBounds)
        {
            _cardSpawner = cardSpawner;
            _cardsTableBounds = cardsTableBounds;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _cardData = GetComponentInParent<IncubatorData>(true);
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

            _currentRecipeSubscription = _cardData.CurrentIncubatorRecipe.Subscribe(OnCurrentRecipeChanged);
        }

        private void StopObserving()
        {
            _currentRecipeSubscription?.Dispose();
        }

        private void OnCurrentRecipeChanged(IncubatorRecipe recipe)
        {
            if (recipe == null || recipe.Cooldown == 0)
            {
                StopProgress();
            }
            else
            {
                StartProgress(_cardData.CurrentIncubatorRecipe.Value.Cooldown);
            }
        }

        protected override void OnProgressCompleted()
        {
            if (_cardData.CurrentIncubatorRecipe.Value == null ||
                _cardData.CurrentIncubatorRecipe.Value.Cooldown == 0) return;

            _cardData.IncubatorCallbacks.onExecutedRecipe?.Invoke(_cardData.CurrentIncubatorRecipe.Value);

            Card cardToSpawn = GetCardToSpawn();
            IncubatorRecipe currentRecipe = _cardData.CurrentIncubatorRecipe.Value;
            ClearRecipeResources();
            SpawnRecipeResult(currentRecipe, cardToSpawn);

            ExecuteActiveRecipe();
        }

        private void ExecuteActiveRecipe()
        {
            OnCurrentRecipeChanged(_cardData.CurrentIncubatorRecipe.Value);
        }

        private void SpawnRecipeResult(IncubatorRecipe recipe, Card card)
        {
            CardData spawnedCard = _cardSpawner.Spawn(card, _cardData.transform.position);

            if (recipe.LinkResultToIncubator)
            {
                spawnedCard.LinkTo(_cardData);
            }
            else
            {
                Vector3 position = _cardsTableBounds.GetRandomPositionInRange(_cardData.Collider.bounds, _minRange, _maxRange);
                spawnedCard.Animations.MoveAnimation.Play(position);
            }

            _cardData.IncubatorCallbacks.onSpawnedResultedCard?.Invoke(card);
        }

        private Card GetCardToSpawn()
        {
            return _cardData.CurrentIncubatorRecipe.Value.Result.Weights.GetByWeight(x => x.Weight).Card;
        }

        private void ClearRecipeResources()
        {
            IncubatorRecipe currentRecipe = _cardData.CurrentIncubatorRecipe.Value;
            List<CardData> bottomCards = _cardData.BottomCards.ToList();

            foreach (var bottomCard in bottomCards)
            {
                if (currentRecipe.RemoveResourcesOnComplete)
                {
                    bottomCard.gameObject.SetActive(false);
                }
                else
                {
                    Vector3 position = _cardsTableBounds.GetRandomPositionInRange(bottomCard.Collider.bounds, _minRange, _maxRange);
                    bottomCard.Animations.MoveAnimation.Play(position);
                }
            }
        }
    }
}
