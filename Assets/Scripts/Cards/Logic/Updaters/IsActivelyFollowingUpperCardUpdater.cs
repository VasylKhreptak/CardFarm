using System;
using Cards.Data;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Logic.Updaters
{
    public class IsActivelyFollowingUpperCardUpdater : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardDataHolder _cardData;

        [Header("Preferences")]
        [SerializeField] private float _updateInterval = 1 / 10f;
        [SerializeField] private float _minDistance = 0.1f;

        private IDisposable _intervalSubscription;
        private IDisposable _isSelectedSubscription;

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
            _cardData.IsActivelyFollowingCard.Value = false;
        }

        #endregion

        private void StartObserving()
        {
            StartObservingSelection();
        }

        private void StopObserving()
        {
            StopObservingSelection();
            StopUpdating();
        }

        private void StartObservingSelection()
        {
            StopObservingSelection();
            _isSelectedSubscription = _cardData.IsSelected
                .Subscribe(OnIsSelectedChanged);
        }

        private void StopObservingSelection()
        {
            _isSelectedSubscription?.Dispose();
        }

        private void OnIsSelectedChanged(bool isSelected)
        {
            if (isSelected)
            {
                StopUpdating();
            }
            else
            {
                StartUpdating();
            }
        }

        private void StartUpdating()
        {
            StopUpdating();
            _intervalSubscription = Observable
                .Interval(TimeSpan.FromSeconds(_updateInterval))
                .DoOnSubscribe(UpdateValue)
                .Subscribe(_ => UpdateValue());
        }

        private void StopUpdating()
        {
            _intervalSubscription?.Dispose();
            _cardData.IsActivelyFollowingCard.Value = false;
        }

        private void UpdateValue()
        {
            bool isActivelyFollowingCard = false;

            CardDataHolder upperCard = _cardData.UpperCard.Value;

            if (upperCard != null)
            {
                Vector3 position = _cardData.transform.position;
                Vector3 targetPosition = _cardData.UpperCard.Value.BottomCardFollowPoint.position;
                targetPosition.y = position.y;

                float distance = Vector3.Distance(position, targetPosition);

                isActivelyFollowingCard = distance > _minDistance;
            }

            _cardData.IsActivelyFollowingCard.Value = isActivelyFollowingCard;
        }
    }
}
