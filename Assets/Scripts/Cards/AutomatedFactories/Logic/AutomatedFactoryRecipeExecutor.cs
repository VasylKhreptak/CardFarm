using System;
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
using Table.Core;
using UniRx;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Cards.AutomatedFactories.Logic
{
    public class AutomatedFactoryRecipeExecutor : ProgressDependentObject
    {
        [Header("References")]
        [SerializeField] private AutomatedCardFactoryData _cardData;
        [SerializeField] private CompatibleCards _compatibleCards;

        [Header("Spawn Preferences")]
        [SerializeField] private float _minRange = 5f;
        [SerializeField] private float _maxRange = 7f;
        [SerializeField] private float _resultedCardMoveDuration = 0.5f;

        private IDisposable _currentRecipeSubscription;

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

            SpawnRecipeResult();
            ClearRecipeResources();

            TryExecuteActiveRecipe();
        }

        private void OnCurrentRecipeChanged(CardFactoryRecipe recipe)
        {
            if (recipe == null)
            {
                StopProgress();
            }
            else
            {
                StartProgress(_cardData.CurrentFactoryRecipe.Value.Cooldown);
            }
        }

        private void TryExecuteActiveRecipe()
        {
            OnCurrentRecipeChanged(_cardData.CurrentFactoryRecipe.Value);
        }

        protected void SpawnRecipeResult()
        {
            Card cardToSpawn = GetCardToSpawn();

            if (_cardsTable.TryGetLowestGroupCard(cardToSpawn, out CardData lowestGroupCard))
            {
                Vector3 position = _cardData.transform.position;
                CardData spawnedCard = _cardSpawner.Spawn(cardToSpawn, position);
                spawnedCard.LinkTo(lowestGroupCard);
            }
            else
            {
                Vector3 position = GetRandomPosition();
                CardData spawnedCard = _cardSpawner.Spawn(cardToSpawn, _cardData.transform.position);
                spawnedCard.Animations.MoveAnimation.Play(position, _resultedCardMoveDuration);
            }
        }

        private Vector3 GetRandomPosition()
        {
            float range = GetRange();

            Vector2 insideUnitCircle = Random.insideUnitCircle.normalized * range;

            Vector3 randomSphere = new Vector3(insideUnitCircle.x, 0f, insideUnitCircle.y);
            return _cardData.transform.position + randomSphere;
        }

        private float GetRange()
        {
            return Random.Range(_minRange, _maxRange);
        }

        private Card GetCardToSpawn()
        {
            return _cardData.CurrentFactoryRecipe.Value.Result.Weights.GetByWeight(x => x.Weight).Card;
        }

        private void ClearRecipeResources()
        {
            int recipeResourcesCount = _cardData.CurrentFactoryRecipe.Value.Resources.Count;
            CardData previousCard = _cardData.BottomCards[recipeResourcesCount];

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
