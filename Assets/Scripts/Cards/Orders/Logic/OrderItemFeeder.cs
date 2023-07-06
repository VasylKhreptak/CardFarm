using System.Collections.Generic;
using System.Linq;
using Cards.Data;
using Cards.Orders.Data;
using UnityEngine;
using Zenject;

namespace Cards.Orders.Logic
{
    public class OrderItemFeeder : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private OrderData _orderData;

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
            StartObservingBottomCards();
        }

        private void OnDisable()
        {
            StopObservingBottomCards();

            _orderData.CurrentCardsCount.Value = 0;
        }

        #endregion

        private void StartObservingBottomCards()
        {
            StopObservingBottomCards();

            _orderData.Callbacks.onBottomCardsListUpdated += OnBottomCardsUpdated;
        }

        private void StopObservingBottomCards()
        {
            _orderData.Callbacks.onBottomCardsListUpdated -= OnBottomCardsUpdated;
        }

        private void OnBottomCardsUpdated()
        {
            TryFeedOrder();
        }

        private void TryFeedOrder()
        {
            List<CardData> bottomCards = _orderData.BottomCards.ToList();

            int leftCards = _orderData.LeftCardsCount.Value;

            int cardsToCollect = Mathf.Min(leftCards, bottomCards.Count);

            for (int i = 0; i < cardsToCollect; i++)
            {
                CardData targetCard = bottomCards[i];

                if (targetCard.Card.Value == _orderData.OrderRequiredCard)
                {
                    targetCard.gameObject.SetActive(false);

                    _orderData.CurrentCardsCount.Value++;
                }
            }
        }
    }
}
