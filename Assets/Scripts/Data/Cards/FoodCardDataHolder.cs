using System;

namespace Data.Cards
{
    [Serializable]
    public class FoodCardDataHolder : SellableCardDataHolder
    {
        public int NutritionalValue;

        public FoodCardDataHolder() : base()
        {
            
        }

        public FoodCardDataHolder(SellableCardDataHolder sellableCardDataHolder) : base(sellableCardDataHolder)
        {
            
        }

        public FoodCardDataHolder(FoodCardDataHolder foodCardDataHolder) : base(foodCardDataHolder)
        {
            NutritionalValue = foodCardDataHolder.NutritionalValue;
        }
    }
}
