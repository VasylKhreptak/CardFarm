using Cards.Data;
using UniRx;

namespace Cards.Chests.SellableChest.Data
{
    public class ChestSellableCardData : SellableCardData
    {
        public ReactiveCollection<SellableCardData> StoredCards = new ReactiveCollection<SellableCardData>();
    }
}
