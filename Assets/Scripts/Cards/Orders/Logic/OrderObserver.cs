﻿using System;
using Cards.Orders.Data;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Orders.Logic
{
    public class OrderObserver : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private OrderData _orderData;

        private IDisposable _leftCardsCountSubscription;

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
            _leftCardsCountSubscription = _orderData.LeftCardsCount.Subscribe(OnLeftCardsCountUpdated);
        }

        private void StopObserving()
        {
            _leftCardsCountSubscription?.Dispose();
        }

        private void OnLeftCardsCountUpdated(int leftCardsCount)
        {
            _orderData.IsOrderCompleted.Value = leftCardsCount == 0;
        }
    }
}
