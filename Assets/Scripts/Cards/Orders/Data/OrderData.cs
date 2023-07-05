using Cards.Data;
using Cards.Orders.Core;
using UniRx;

namespace Cards.Orders.Data
{
    public class OrderData : CardData
    {
        public ReactiveProperty<Order> Order = new ReactiveProperty<Order>();
        public BoolReactiveProperty IsOrderCompleted = new BoolReactiveProperty();
    }
}
