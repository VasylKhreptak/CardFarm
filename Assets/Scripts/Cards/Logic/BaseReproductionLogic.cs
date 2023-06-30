using System;
using System.Collections.Generic;
using System.Linq;
using Cards.Core;
using Cards.Data;
using Cards.Logic.Spawn;
using Constraints.CardTable;
using Extensions;
using ProgressLogic.Core;
using ScriptableObjects.Scripts.Cards.ReproductionRecipes;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Logic
{
    public class BaseReproductionLogic : ProgressDependentObject, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

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
            _cardData = GetComponentInParent<CardData>(true);
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

            _currentRecipeSubscription = _cardData.CurrentReproductionRecipe.Subscribe(OnCurrentRecipeUpdated);
        }

        private void StopObserving()
        {
            _currentRecipeSubscription?.Dispose();
        }

        private void OnCurrentRecipeUpdated(CardReproductionRecipe recipe)
        {
            if (recipe == null || recipe.Cooldown == 0f)
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
            SpawnResult();

            UnlinkResources();
        }

        private void UnlinkResources()
        {
            List<CardData> groupCards = _cardData.GroupCards.ToList();

            foreach (var groupCard in groupCards)
            {
                groupCard.UnlinkFromUpper();

                Vector3 position = _cardsTableBounds.GetRandomPositionInRange(_cardData.Collider.bounds, _minRange, _maxRange);

                groupCard.Animations.JumpAnimation.Play(position);
            }
        }

        private void SpawnResult()
        {
            Card cardToSpawn = _cardData.CurrentReproductionRecipe.Value.Results.GetByWeight(x => x.Weight).Card;

            _cardSpawner.SpawnAndMove(cardToSpawn, _cardData.transform.position);
        }
    }
}
