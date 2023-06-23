using System;
using Cards.Data;
using Table;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Logic.Updaters
{
    public class CanBeUnderSelectedCardUpdater : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        private IDisposable _isCardCompatibleSubscription;
        private IDisposable _cardsInfoSubscription;

        private CurrentSelectedCardHolder _currentSelectedCardHolder;

        [Inject]
        private void Constructor(CurrentSelectedCardHolder currentSelectedCardHolder)
        {
            _currentSelectedCardHolder = currentSelectedCardHolder;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            _cardData ??= GetComponentInParent<CardData>();
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
            StartObservingIfCompatible();
        }

        private void StopObserving()
        {
            StopObservingIfCompatible();
            StopObservingCardsInfo();
        }

        private void StartObservingIfCompatible()
        {
            StopObservingIfCompatible();

            _isCardCompatibleSubscription = _cardData.IsCompatibleWithSelectedCard.Subscribe(IsCompatibleValueChanged);
        }

        private void StopObservingIfCompatible()
        {
            _isCardCompatibleSubscription?.Dispose();
        }

        private void IsCompatibleValueChanged(bool isCompatible)
        {
            if (isCompatible == false || _cardData == _currentSelectedCardHolder.SelectedCard.Value)
            {
                _cardData.CanBeUnderSelectedCard.Value = false;
                StopObservingCardsInfo();
                return;
            }

            StartObservingCardsInfo();
        }

        private void StartObservingCardsInfo()
        {
            StopObservingCardsInfo();

            _cardsInfoSubscription = Observable.CombineLatest(
                    _cardData.IsLowestGroupCard,
                    _currentSelectedCardHolder.SelectedCard.Value.GroupID,
                    _cardData.GroupID,
                    (isLowestGroupCard, selectedCardGroupID, cardGroupID) => (isLowestGroupCard, selectedCardGroupID, cardGroupID))
                .Subscribe(tuple =>
                    OnCardsInfoUpdated(tuple.isLowestGroupCard, tuple.selectedCardGroupID, tuple.cardGroupID));
        }

        private void StopObservingCardsInfo()
        {
            _cardsInfoSubscription?.Dispose();
        }

        private void OnCardsInfoUpdated(bool isLowestGroupCard, int selectedCardGroupID, int cardGroupID)
        {
            _cardData.CanBeUnderSelectedCard.Value =
                isLowestGroupCard
                && selectedCardGroupID != cardGroupID
                && _cardData != _currentSelectedCardHolder.SelectedCard.Value;
        }
    }
}
