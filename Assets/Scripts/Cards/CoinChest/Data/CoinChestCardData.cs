using Cards.Data;
using UniRx;

namespace Cards.CoinChest.Data
{
    public class CoinChestCardData : CardData
    {
        public IntReactiveProperty Coins = new IntReactiveProperty();
    }
}
