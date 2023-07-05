using System.Collections.Generic;
using Cards.Orders.Core;
using SRF;
using UnityEngine;

namespace ScriptableObjects.Scripts.Cards.Orders
{
    [CreateAssetMenu(fileName = "CardOrders", menuName = "ScriptableObjects/CardOrders")]
    public class CardOrders : ScriptableObject
    {
        [Header("Preferences")]
        [SerializeField] private List<Order> _orders = new List<Order>();

        public Order GetRandomOrder() => _orders.Random();
    }
}
