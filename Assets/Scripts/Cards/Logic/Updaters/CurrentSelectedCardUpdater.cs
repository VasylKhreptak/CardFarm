using System;
using Cards.Data;
using Table;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Logic.Updaters
{
    public class CurrentSelectedCardUpdater : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        private IDisposable _isCardSelectedSubscription;

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
            StopObserving();

            _isCardSelectedSubscription = _cardData.IsSelectedCard.Subscribe(OnCardStateUpdated);
        }

        private void StopObserving()
        {
            _isCardSelectedSubscription?.Dispose();
            OnCardStateUpdated(false);
        }

        private void OnCardStateUpdated(bool isSelected)
        {
            if (isSelected)
            {
                _currentSelectedCardHolder.SelectedCard.Value = _cardData;
            }
            else if (_currentSelectedCardHolder.SelectedCard.Value == _cardData)
            {
                _currentSelectedCardHolder.SelectedCard.Value = null;
            }
        }
    }
}
