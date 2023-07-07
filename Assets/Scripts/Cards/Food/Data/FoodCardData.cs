using Cards.Data;
using UniRx;

namespace Cards.Food.Data
{
    public class FoodCardData : SellableCardData
    {
        public IntReactiveProperty NutritionalValue = new IntReactiveProperty();
        public IntReactiveProperty MaxNutritionalValue = new IntReactiveProperty();
    }
}
