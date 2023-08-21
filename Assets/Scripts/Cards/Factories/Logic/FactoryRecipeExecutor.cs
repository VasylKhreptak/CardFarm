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

        [Header("Preferences")]
        [SerializeField] private float _timeScale = 3f;
        [SerializeField] private float _minSpreadRange = 5f;
        [SerializeField] private float _maxSpreadRange = 7f;

        private IDisposable _currentRecipeSubscription;

        private List<Card> _cardsToSpawn = new List<Card>();

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

            _cardData.GearsDrawer.OnDrawnCheckmark += OnDrawnCheckmark;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            StopObserving();

            _cardData.GearsDrawer.OnDrawnCheckmark -= OnDrawnCheckmark;
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

            _cardsToSpawn.Clear();

            for (int i = 0; i < _cardData.CurrentFactoryRecipe.Value.ResultCount; i++)
            {
                _cardsToSpawn.Add(GetCardToSpawn());
            }
        }

        private void OnDrawnCheckmark()
        {
            SpawnRecipeResults();
            TryClearRecipeResources();

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
                StartProgress(_cardData.CurrentFactoryRecipe.Value.Cooldown / _timeScale);
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
            
            for (int i = 0; i < _cardsToSpawn.Count; i++)
            {
                CardData spawnedCard = SpawnRecipeResult(_cardsToSpawn[i]);

                if (i == _cardsToSpawn.Count - 1)
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

            _cardData.Callbacks.onSpawnedRecipeResult?.Invoke(cardToSpawn);

            return spawnedCard;
        }

        private CardData SpawnRecipeResult(Card card)
        {
            CardData spawnedCard = _cardSpawner.SpawnAndMove(card, _cardData.transform.position);

            _cardData.Callbacks.onSpawnedRecipeResult?.Invoke(card);

            return spawnedCard;
        }

        private Card GetCardToSpawn()
        {
            return _cardData.CurrentFactoryRecipe.Value.Result.Weights.GetByWeight(x => x.Weight).Card;
        }

        private void TryClearRecipeResources()
        {
            FactoryRecipe currentRecipe = _cardData.CurrentFactoryRecipe.Value;

            if (currentRecipe.RemoveResourcesOnComplete)
            {
                ClearRecipeResources();
            }
            else
            {
                SpreadRecipeResources();
            }
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

        private void SpreadRecipeResources()
        {
            int recipeResourcesCount = _cardData.CurrentFactoryRecipe.Value.Resources.Count;

            CardData previousCard = null;

            if (_cardData.BottomCards.Count > recipeResourcesCount)
            {
                previousCard = _cardData.BottomCards[recipeResourcesCount];
            }

            List<CardData> resourcesToSpread = _cardData.BottomCards.Take(recipeResourcesCount).ToList();

            foreach (CardData resourceCard in resourcesToSpread)
            {
                resourceCard.Separate();
                resourceCard.Animations.MoveAnimation.PlayRandomly(_minSpreadRange, _maxSpreadRange);
            }

            TryLinkCardToTop(previousCard);
        }

        private void TryLinkCardToTop(CardData card)
        {
            if (_compatibleCards.IsCompatibleByCategory(card, _cardData))
            {
                card.LinkTo(_cardData);
            }
        }
    }
}
