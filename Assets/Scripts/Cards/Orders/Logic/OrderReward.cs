using System;
using System.Collections.Generic;
using System.Linq;
using Cards.Core;
using Cards.Data;
using Cards.Logic.Spawn;
using Cards.Orders.Data;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Orders.Logic
{
    public class OrderReward : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private OrderData _orderData;

        private IDisposable _isOrderCompletedSubscription;

        private CardSpawner _cardSpawner;

        [Inject]
        private void Constructor(CardSpawner cardSpawner)
        {
            _cardSpawner = cardSpawner;
        }

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
            StartObservingOrderCompletion();
        }

        private void OnDisable()
        {
            StopObservingOrderCompletion();
        }

        #endregion

        private void StartObservingOrderCompletion()
        {
            StopObservingOrderCompletion();
            _isOrderCompletedSubscription = _orderData.IsOrderCompleted.Subscribe(OnOrderCompletionStateUpdated);
        }

        private void StopObservingOrderCompletion()
        {
            _isOrderCompletedSubscription?.Dispose();
        }

        private void OnOrderCompletionStateUpdated(bool isCompleted)
        {
            if (isCompleted)
            {
                OnOrderCompleted();
            }
        }

        private void OnOrderCompleted()
        {
            if (_orderData.Order.Value == null) return;

            Card cardToSpawn = _orderData.Order.Value.RewardCard;

            _cardSpawner.SpawnAndMove(cardToSpawn, _orderData.transform.position);

            List<CardData> bottomCards = _orderData.BottomCards.ToList();

            foreach (var bottomCard in bottomCards)
            {
                bottomCard.gameObject.SetActive(false);
            }

            _orderData.gameObject.SetActive(false);
        }
    }
}
