using System;
using Cards.Data;
using Cards.Factories.Data;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Logic.Updaters
{
    public class IsTakingPartInRecipeUpdater : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardDataHolder _cardData;

        private IDisposable _firstGroupCardSubscription;
        private CompositeDisposable _firstGroupCardSubscriptions = new CompositeDisposable();

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _cardData = GetComponentInParent<CardDataHolder>(true);
        }

        private void OnEnable()
        {
            StartObserving();
        }

        private void OnDisable()
        {
            StopObserving();
            _cardData.IsTakingPartInRecipe.Value = false;
        }

        #endregion

        private void StartObserving()
        {
            _firstGroupCardSubscription = _cardData.FirstGroupCard.Subscribe(OnFirstGroupCardUpdated);
        }

        private void StopObserving()
        {
            _firstGroupCardSubscription?.Dispose();
            ClearGroupCardsSubscriptions();
        }

        private void OnFirstGroupCardUpdated(CardDataHolder firstGroupCard)
        {
            ClearGroupCardsSubscriptions();

            if (firstGroupCard == null)
            {
                SetState(false);
                return;
            }

            if (firstGroupCard.IsResourceNode)
            {
                SetState(true);
                return;
            }

            TryStartObservingCurrentRecipe(firstGroupCard);
            TryStartObservingReproductionRecipe(firstGroupCard);
            TryStartObservingFactoryRecipe(firstGroupCard);
        }

        private void SetState(bool state)
        {
            _cardData.IsTakingPartInRecipe.Value = state;
        }

        private void TryStartObservingCurrentRecipe(CardDataHolder cardData)
        {
            cardData.CurrentRecipe.Subscribe(recipe =>
            {
                SetState(recipe != null && recipe.Cooldown > 0);
            }).AddTo(_firstGroupCardSubscriptions);
        }

        private void TryStartObservingReproductionRecipe(CardDataHolder cardData)
        {
            cardData.CurrentReproductionRecipe.Subscribe(recipe =>
            {
                SetState(recipe != null && recipe.Cooldown > 0);
            }).AddTo(_firstGroupCardSubscriptions);
        }

        private void TryStartObservingFactoryRecipe(CardDataHolder cardData)
        {
            if (cardData.IsAutomatedFactory)
            {
                FactoryData factoryData = cardData as FactoryData;

                factoryData.CurrentFactoryRecipe.Subscribe(recipe =>
                {
                    SetState(recipe != null && recipe.Cooldown > 0);
                }).AddTo(_firstGroupCardSubscriptions);
            }
        }

        private void ClearGroupCardsSubscriptions()
        {
            _firstGroupCardSubscriptions.Clear();
        }
    }
}
