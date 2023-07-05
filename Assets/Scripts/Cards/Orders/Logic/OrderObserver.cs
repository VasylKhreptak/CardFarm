using System;
using System.Collections.Generic;
using Cards.Core;
using Cards.Data;
using Cards.Orders.Core;
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

        private IDisposable _orderSubscription;

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

            StartObservingOrder();
        }

        private void StopObserving()
        {
            StopObservingOrder();
            StopObservingBottomCards();
            MarkOrderCompletion(false);
        }

        private void StartObservingOrder()
        {
            StopObservingOrder();
            _orderSubscription = _orderData.Order.Subscribe(OnOrderUpdated);
        }

        private void StopObservingOrder()
        {
            _orderSubscription?.Dispose();
        }

        private void OnOrderUpdated(Order order)
        {
            if (order == null)
            {
                StopObservingBottomCards();
                MarkOrderCompletion(false);
                return;
            }

            StartObservingBottomCards();
        }

        private void StartObservingBottomCards()
        {
            StopObservingBottomCards();
            _orderData.Callbacks.onBottomCardsListUpdated += OnBottomCardsUpdated;
        }

        private void StopObservingBottomCards()
        {
            _orderData.Callbacks.onBottomCardsListUpdated -= OnBottomCardsUpdated;
            MarkOrderCompletion(false);
        }

        private void OnBottomCardsUpdated()
        {
            List<CardData> bottomCards = _orderData.BottomCards;

            if (bottomCards.Count != 1)
            {
                MarkOrderCompletion(false);
                return;
            }

            Card bottomCard = bottomCards[0].Card.Value;

            if (_orderData.Order.Value.TargetCard == bottomCard)
            {
                MarkOrderCompletion(true);
            }
        }

        private void MarkOrderCompletion(bool isCompleted)
        {
            _orderData.IsOrderCompleted.Value = isCompleted;
        }
    }
}
