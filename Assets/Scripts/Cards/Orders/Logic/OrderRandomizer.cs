using Cards.Orders.Data;
using ScriptableObjects.Scripts.Cards.Orders;
using ScriptableObjects.Scripts.DataPairs;
using UnityEngine;
using Zenject;

namespace Cards.Orders.Logic
{
    public class OrderRandomizer : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private OrderData _orderData;

        [Header("Preferences")]
        [SerializeField] private CardSpritePairs _cardSpritePairs;
        [SerializeField] private CardOrders _cardOrders;

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
            RandomizeOrder();
        }

        #endregion

        private void RandomizeOrder()
        {
            _orderData.OrderRequiredCard = _cardOrders.GetRandomCard();
            _orderData.OrderIcon.Value = _cardSpritePairs.GetValue(_orderData.OrderRequiredCard);
            _orderData.TargetCardsCount.Value = _cardOrders.GetRandomCardsCount();
            _orderData.Rewards = _cardOrders.Rewards;
        }
    }
}
