using System;
using Cards.Orders.Data;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Orders.Logic
{
    public class OrderIconRevealer : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private OrderData _orderData;

        [Header("Preferences")]
        [SerializeField] private GameObject _defaultIconObject;
        [SerializeField] private GameObject _orderIconObject;
        [SerializeField] private float _showTime = 2f;

        private IDisposable _timerSubscription;

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _orderData = GetComponentInParent<OrderData>(true);
        }

        private void OnEnable()
        {
            SetOrderIronState(false);
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

            StartObservingClick();
        }

        private void StopObserving()
        {
            StopObservingClick();
            _timerSubscription?.Dispose();
        }

        private void StartObservingClick()
        {
            StopObservingClick();

            _orderData.Callbacks.onClicked += OnClicked;
        }

        private void StopObservingClick()
        {
            _orderData.Callbacks.onClicked -= OnClicked;
        }

        private void OnClicked()
        {
            RevealOrderIcon();
        }

        private void RevealOrderIcon()
        {
            SetOrderIronState(true);

            _timerSubscription?.Dispose();
            _timerSubscription = Observable.Timer(TimeSpan.FromSeconds(_showTime))
                .Subscribe(_ => SetOrderIronState(false));
        }

        private void SetOrderIronState(bool enabled)
        {
            _defaultIconObject.SetActive(!enabled);
            _orderIconObject.SetActive(enabled);
        }
    }
}
