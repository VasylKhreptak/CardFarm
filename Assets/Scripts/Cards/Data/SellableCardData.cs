using UniRx;

namespace Cards.Data
{
    public class SellableCardData : CardData
    {
        public IntReactiveProperty Price = new IntReactiveProperty();
    }
}
