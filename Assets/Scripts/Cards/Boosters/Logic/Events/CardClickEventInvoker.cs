using System;
using Cards.Data;
using UniRx;
using UnityEngine;

namespace Cards.Boosters.Logic.Events
{
    public class CardClickEventInvoker : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        [Header("Preferences")]
        [SerializeField] private float _maxCardDistance = 0.1f;

        private IDisposable _mouseDownSubscription;
        private IDisposable _mouseUpSubscription;

        private Vector2 _previousCardPosition;

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
            Vector3 cardPosition = _cardData.transform.position;
            _previousCardPosition = new Vector2(cardPosition.x, cardPosition.z);
        }

        private void OnMouseUp()
        {
            Vector3 cardPosition = _cardData.transform.position;
            Vector2 currentCardPosition = new Vector2(cardPosition.x, cardPosition.z);

            if (Vector2.Distance(currentCardPosition, _previousCardPosition) < _maxCardDistance)
            {
                _cardData.Callbacks.onClicked?.Invoke();
            }
        }
    }
}
