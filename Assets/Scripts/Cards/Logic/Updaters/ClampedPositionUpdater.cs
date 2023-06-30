using System;
using Cards.Data;
using Constraints.CardTable;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Logic.Updaters
{
    public class ClampedPositionUpdater : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        [Header("Preferences")]
        [SerializeField] private Vector3 _boundExpand = new Vector3(0.02f, 0.02f, 0.02f);

        private IDisposable _isInsideTableSubscription;
        private IDisposable _updateSubscription;

        private CardsTableBounds _cardsTableBounds;

        [Inject]
        private void Constructor(CardsTableBounds cardsTableBounds)
        {
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
            _updateSubscription = Observable
                .EveryUpdate()
                .DoOnSubscribe(OnUpdateTick)
                .Subscribe(_ => OnUpdateTick());
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
            Bounds bounds = _cardData.Collider.bounds;
            bounds.Expand(_boundExpand);
            _cardData.ClampedPosition.Value = _cardsTableBounds.Clamp(bounds);
        }
    }
}
