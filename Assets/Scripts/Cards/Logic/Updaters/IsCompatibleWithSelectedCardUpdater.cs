using System;
using Cards.Data;
using CardsTable;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Logic.Updaters
{
    public class IsCompatibleWithSelectedCardUpdater : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        private IDisposable _subscription;

        private CurrentSelectedCardHolder _currentSelectedCardHolder;

        [Inject]
        private void Constructor(CurrentSelectedCardHolder currentSelectedCardHolder)
        {
            _currentSelectedCardHolder = currentSelectedCardHolder;
        }

        #region MonoBehaviour

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
            StopObserving();

            _subscription = Observable
                .CombineLatest(
                    _cardData.IsLowestGroupCard,
                    _currentSelectedCardHolder.SelectedCard,
                    (isLowestGroupCard, selectedCard) => (isLowestGroupCard, selectedCard))
                .Subscribe(tuple =>
                {
                    OnConditionUpdated(tuple.isLowestGroupCard, tuple.selectedCard);
                });
        }

        private void StopObserving()
        {
            _subscription?.Dispose();
        }

        private void OnConditionUpdated(bool islLowestGroupCard, CardData selectedCard)
        {
            _cardData.IsCompatibleWithSelectedCard.Value = islLowestGroupCard && selectedCard != _cardData;
        }
    }
}
