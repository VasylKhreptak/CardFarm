using System;
using Cards.Data;
using UniRx;
using UnityEngine;

namespace Cards.Logic.Events
{
    public class CardClickEventInvoker : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        [Header("Preferences")]
        [SerializeField] private float _maxDistance = 0.1f;

        private IDisposable _mouseDownSubscription;
        private IDisposable _mouseUpSubscription;

        private Vector2 _previousClickPosition;

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
            StartObservingMouseDown();
            StartObservingMouseUp();
        }

        private void StopObserving()
        {
            StopObservingMouseDown();
            StopObservingMouseUp();
        }

        private void StartObservingMouseDown()
        {
            StopObservingMouseDown();
            _mouseDownSubscription = _cardData.MouseTrigger.OnMouseDownAsObservable().Subscribe(_ => OnMouseDown());
        }

        private void StopObservingMouseDown()
        {
            _mouseDownSubscription?.Dispose();
        }

        private void StartObservingMouseUp()
        {
            StopObservingMouseUp();
            _mouseUpSubscription = _cardData.MouseTrigger.OnMouseUpAsObservable().Subscribe(_ => OnMouseUp());
        }

        private void StopObservingMouseUp()
        {
            _mouseUpSubscription?.Dispose();
        }

        private void OnMouseDown()
        {
            _previousClickPosition = Input.mousePosition;
        }

        private void OnMouseUp()
        {
            Vector2 currentClickPosition = Input.mousePosition;

            if (Vector2.Distance(currentClickPosition, _previousClickPosition) < _maxDistance)
            {
                _cardData.Callbacks.onClicked?.Invoke();
            }
        }
    }
}
