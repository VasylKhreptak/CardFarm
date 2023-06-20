using Cards.Data;
using UniRx;

namespace Cards.Chests.Core.Data
{
    public class ChestCardData : SellableCardData
    {
        public ReactiveCollection<SellableCardData> StoredCards = new ReactiveCollection<SellableCardData>();
    }
}
