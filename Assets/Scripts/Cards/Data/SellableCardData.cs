using System;
using UniRx;

namespace Cards.Data
{
    [Serializable]
    public class SellableCardData : CardData
    {
        public IntReactiveProperty Price = new IntReactiveProperty();
    }
}
