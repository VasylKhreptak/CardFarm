using System;
using Cards.Data;
using UniRx;
using UnityEngine;

namespace Cards.Logic.Updaters
{
    public class IsSelectedCardUpdater : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        private IDisposable _mouseDownSubscription;
        private IDisposable _mouseUpSubscription;

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
            _mouseDownSubscription = _cardData.MouseTrigger.OnMouseDownAsObservable().Subscribe(_ =>
            {
                _cardData.IsSelectedCard.Value = true;
            });
        }

        private void StopObservingMouseDown()
        {
            _mouseDownSubscription?.Dispose();
        }

        private void StartObservingMouseUp()
        {
            StopObservingMouseUp();
            _mouseUpSubscription = _cardData.MouseTrigger.OnMouseUpAsObservable().Subscribe(_ =>
            {
                _cardData.IsSelectedCard.Value = false;
            });
        }

        private void StopObservingMouseUp()
        {
            _mouseUpSubscription?.Dispose();
        }
    }
}
