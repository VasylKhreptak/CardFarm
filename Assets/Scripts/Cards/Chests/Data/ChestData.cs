using Cards.Core;
using Cards.Data;
using UniRx;

namespace Cards.Chests.Data
{
    public class ChestData : SellableCardData
    {
        public ReactiveProperty<Card?> ChestType = new ReactiveProperty<Card?>();
        public ReactiveCollection<SellableCardData> StoredCards = new ReactiveCollection<SellableCardData>();
        public IntReactiveProperty Size = new IntReactiveProperty();
        public IntReactiveProperty Capacity = new IntReactiveProperty();
    }
}
