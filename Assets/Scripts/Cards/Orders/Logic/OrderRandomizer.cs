using Cards.Orders.Data;
using ScriptableObjects.Scripts.Cards.Orders;
using UnityEngine;
using Zenject;

namespace Cards.Orders.Logic
{
    public class OrderRandomizer : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private OrderData _orderData;

        [Header("Preferences")]
        [SerializeField] private CardOrders _orders;

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
            _orderData.Order.Value = _orders.GetRandomOrder();
        }
    }
}
