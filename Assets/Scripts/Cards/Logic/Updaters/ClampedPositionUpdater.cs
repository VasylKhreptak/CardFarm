using System;
using Cards.Data;
using Constraints.CardTable;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Logic.Updaters
{
    public class ClampedPositionUpdater : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        private IDisposable _isInsideTableSubscription;
        private IDisposable _updateSubscription;

        private CardsTableConstraints _cardsTableConstraints;

        [Inject]
        private void Constructor(CardsTableConstraints cardsTableConstraints)
        {
            _cardsTableConstraints = cardsTableConstraints;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            _cardData ??= GetComponentInParent<CardData>();
        }

        private void OnEnable()
        {
            StartObservingIfInsideTable();
        }

        private void OnDisable()
        {
            StopObservingIfInsideTable();
            StopUpdatingClampedPosition();
        }

        #endregion

        private void StartObservingIfInsideTable()
        {
            StopObservingIfInsideTable();

            _isInsideTableSubscription = _cardData.IsInsideCardsTable.Subscribe(IsInsideTableChanged);
        }

        private void StopObservingIfInsideTable()
        {
            _isInsideTableSubscription?.Dispose();
        }

        private void IsInsideTableChanged(bool isInsideTable)
        {
            if (isInsideTable == false)
            {
                StartUpdatingClampedPosition();
            }
            else
            {
                StopUpdatingClampedPosition();
            }
        }

        private void StartUpdatingClampedPosition()
        {
            StopUpdatingClampedPosition();
            _updateSubscription = Observable.EveryUpdate().Subscribe(_ => OnUpdateTick());
        }

        private void StopUpdatingClampedPosition()
        {
            _updateSubscription?.Dispose();
        }

        private void OnUpdateTick()
        {
            UpdateClampedPosition();
        }

        private void UpdateClampedPosition()
        {
            _cardData.ClampedPosition.Value = _cardsTableConstraints.Clamp(_cardData.Collider.bounds);
        }
    }
}
