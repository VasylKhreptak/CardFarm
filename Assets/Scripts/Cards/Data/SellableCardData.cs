using UniRx;

namespace Cards.Data
{
    public class SellableCardData : CardDataHolder
    {
        public IntReactiveProperty Price = new IntReactiveProperty();
    }
}
