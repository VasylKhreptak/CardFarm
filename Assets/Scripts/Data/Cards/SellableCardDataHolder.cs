using System;
using Data.Cards.Core;

namespace Data.Cards
{
    [Serializable]
    public class SellableCardDataHolder : CardDataHolder
    {
        public int Price;

        public SellableCardDataHolder()
        {

        }

        public SellableCardDataHolder(CardDataHolder cardDataHolder) : base(cardDataHolder)
        {

        }

        public SellableCardDataHolder(SellableCardDataHolder sellableCardDataHolder) : base(sellableCardDataHolder)
        {
            Price = sellableCardDataHolder.Price;
        }
    }
}
