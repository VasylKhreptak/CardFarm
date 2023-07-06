using System;
using Cards.AutomatedFactories.Data;
using Cards.Data;
using Cards.Incubators.Data;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Logic.Updaters
{
    public class IsTakingPartInRecipeUpdater : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        private IDisposable _firstGroupCardSubscription;
        private CompositeDisposable _firstGroupCardSubscriptions = new CompositeDisposable();

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

        private void OnDisable()
        {
            StopObserving();
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

        private void OnFirstGroupCardUpdated(CardData firstGroupCard)
        {
            ClearGroupCardsSubscriptions();

            if (firstGroupCard == null)
            {
                SetState(false);
                return;
            }

            TryStartObservingCurrentRecipe(firstGroupCard);
            TryStartObservingReproductionRecipe(firstGroupCard);
            TryStartObservingFactoryRecipe(firstGroupCard);
            TryStartObservingIncubatorRecipe(firstGroupCard);
        }

        private void SetState(bool state)
        {
            _cardData.IsTakingPartInRecipe.Value = state;
        }

        private void TryStartObservingCurrentRecipe(CardData cardData)
        {
            cardData.CurrentRecipe.Subscribe(recipe =>
            {
                SetState(recipe != null && recipe.Cooldown > 0);
            }).AddTo(_firstGroupCardSubscriptions);
        }

        private void TryStartObservingReproductionRecipe(CardData cardData)
        {
            cardData.CurrentReproductionRecipe.Subscribe(recipe =>
            {
                SetState(recipe != null && recipe.Cooldown > 0);
            }).AddTo(_firstGroupCardSubscriptions);
        }

        private void TryStartObservingFactoryRecipe(CardData cardData)
        {
            if (cardData.IsAutomatedFactory)
            {
                AutomatedCardFactoryData factoryData = cardData as AutomatedCardFactoryData;

                factoryData.CurrentFactoryRecipe.Subscribe(recipe =>
                {
                    SetState(recipe != null && recipe.Cooldown > 0);
                }).AddTo(_firstGroupCardSubscriptions);
            }
        }

        private void TryStartObservingIncubatorRecipe(CardData cardData)
        {
            if (cardData.IsIncubator)
            {
                IncubatorData incubatorData = cardData as IncubatorData;

                incubatorData.CurrentIncubatorRecipe.Subscribe(recipe =>
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
