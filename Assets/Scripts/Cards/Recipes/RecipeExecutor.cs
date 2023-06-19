using System;
using System.Collections.Generic;
using System.Linq;
using Cards.Core;
using Cards.Data;
using Cards.Logic.Spawn;
using Extensions;
using ProgressLogic.Core;
using ScriptableObjects.Scripts.Cards.Recipes;
using Table.Core;
using UniRx;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Cards.Recipes
{
    public class RecipeExecutor : ProgressDependentObject
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        [Header("Spawn Preferences")]
        [SerializeField] private float _minRange = 5f;
        [SerializeField] private float _maxRange = 7f;

        private IDisposable _recipeSubscription;

        private CardsTable _cardsTable;
        private CardSpawner _cardSpawner;

        [Inject]
        private void Constructor(CardsTable cardsTable, CardSpawner cardSpawner)
        {
            _cardsTable = cardsTable;
            _cardSpawner = cardSpawner;
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
            if (_cardData.CurrentRecipe.Value == null ||
                _cardData.CurrentRecipe.Value.Cooldown == 0) return;

            SpawnRecipeResult();
            ClearRecipeResources();
            _cardData.CurrentRecipe.Value = null;
        }

        private void SpawnRecipeResult()
        {
            Card cardToSpawn = _cardData.CurrentRecipe.Value.Result.Weights.GetByWeight(x => x.Weight).Card;

            if (_cardsTable.TryGetLowestGroupCard(cardToSpawn, out CardData lowestGroupCard))
            {
                Vector3 position = _cardData.transform.position;
                CardData spawnedCard = _cardSpawner.Spawn(cardToSpawn, position);
                spawnedCard.LinkTo(lowestGroupCard);
            }
            else
            {
                Vector3 position = GetRandomPosition();
                _cardSpawner.Spawn(cardToSpawn, position);
            }
        }

        private void ClearRecipeResources()
        {
            CardData firstWorker = GetFirstWorker();
            CardData lastResource = GetLastResource();
            List<CardData> resourcesToRemove = GetResourcesToRemove();

            foreach (var resourceToRemove in resourcesToRemove)
            {
                resourceToRemove.gameObject.SetActive(false);
            }

            firstWorker.LinkTo(lastResource);
        }

        protected Vector3 GetRandomPosition()
        {
            float range = GetRange();

            Vector2 insideUnitCircle = Random.insideUnitCircle.normalized * range;

            Vector3 randomSphere = new Vector3(insideUnitCircle.x, 0f, insideUnitCircle.y);
            return _cardData.transform.position + randomSphere;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(_cardData.transform.position, _minRange);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_cardData.transform.position, _maxRange);
        }

        private float GetRange()
        {
            return Random.Range(_minRange, _maxRange);
        }

        private CardData GetFirstWorker()
        {
            return _cardData.GroupCards.First(x => x.IsWorker);
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
            List<CardData> resources = new List<CardData>(_cardData.CurrentRecipe.Value.Resources.Count);

            _cardData.CurrentRecipe.Value.Resources.ForEach(x =>
            {
                resources.Add(_cardData.GroupCards.Last(card => card.Card.Value == x));
            });

            return resources;
        }
    }
}
