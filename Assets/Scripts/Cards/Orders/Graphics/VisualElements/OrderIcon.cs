using System;
using Cards.Orders.Data;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Cards.Orders.Graphics.VisualElements
{
    public class OrderIcon : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private Image _image;
        [SerializeField] private OrderData _orderData;

        private IDisposable _orderSubscription;

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _orderData = GetComponentInParent<OrderData>(true);
            _image = GetComponent<Image>();
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

            StartObservingOrder();
        }

        private void StopObserving()
        {
            StopObservingOrder();
        }

        private void StartObservingOrder()
        {
            StopObservingOrder();
            _orderSubscription = _orderData.OrderIcon.Subscribe(OnOrderIconChanged);
        }

        private void StopObservingOrder()
        {
            _orderSubscription?.Dispose();
        }

        private void OnOrderIconChanged(Sprite icon)
        {
            _image.sprite = icon;
        }
    }
}
