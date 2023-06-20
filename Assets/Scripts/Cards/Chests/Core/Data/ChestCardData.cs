using Cards.Data;
using UniRx;

namespace Cards.Chests.Core.Data
{
    public class ChestCardData : SellableCardData
    {
        public IntReactiveProperty PriceForOneCard = new IntReactiveProperty(0);
        public IntReactiveProperty ItemsCount = new IntReactiveProperty(0);
    }
}
