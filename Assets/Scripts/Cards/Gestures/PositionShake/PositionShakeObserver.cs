using System;
using Cards.Data;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Gestures.PositionShake
{
    public class PositionShakeObserver : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardDataHolder _cardData;

        [Header("Preferences")]
        [SerializeField] private float _updateInterval;
        [SerializeField] private int _minShakeCount = 3;

        private BoolReactiveProperty _isShaking = new BoolReactiveProperty(false);

        private IDisposable _isSelectedSubscription;
        private IDisposable _intervalSubscription;

        private Vector2 _previousPosition;
        private int _shakeCount;

        public IReadOnlyReactiveProperty<bool> IsShaking => _isShaking;

        #region MonoBehavour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _cardData = GetComponentInParent<CardDataHolder>(true);
        }

        // private void OnEnable()
        // {
        //     StartObservingIfSelected();
        // }
        //
        // private void OnDisable()
        // {
        //     StopObservingIfSelected();
        //     StopObservingShake();
        // }

        #endregion

        private void StartObservingIfSelected()
        {
            StopObservingIfSelected();
            _isSelectedSubscription = _cardData.IsSelected.Subscribe(OnIsSelectedChanged).AddTo(this);
        }

        private void StopObservingIfSelected()
        {
            _isSelectedSubscription?.Dispose();
        }

        private void OnIsSelectedChanged(bool isSelected)
        {
            if (isSelected)
            {
                StartObservingShake();
            }
            else
            {
                StopObservingShake();
            }
        }

        private void StartObservingShake()
        {
            UpdatePreviousPosition();

            StopObservingShake();
            _intervalSubscription = Observable
                .Interval(TimeSpan.FromSeconds(_updateInterval))
                .DoOnSubscribe(OnTick)
                .Subscribe(_ => OnTick());
        }

        private void StopObservingShake()
        {
            _intervalSubscription?.Dispose();
            _shakeCount = 0;
        }

        private void OnTick()
        {
            Vector2 position = Input.mousePosition;

            if (Mathf.Sign(position.x) != Mathf.Sign(_previousPosition.x) ||
                Mathf.Sign(position.y) != Mathf.Sign(_previousPosition.y))
            {
                _shakeCount++;
            }
            else
            {
                _shakeCount = 0;
            }

            _isShaking.Value = _shakeCount >= _minShakeCount;

            _previousPosition = position;
        }

        private void UpdatePreviousPosition()
        {
            _previousPosition = Input.mousePosition;
        }
    }
}
