using System;
using UniRx;

namespace Cards.Data
{
    [Serializable]
    public class FoodCardData : SellableCardData
    {
        public IntReactiveProperty NutritionalValue = new IntReactiveProperty();
    }
}
