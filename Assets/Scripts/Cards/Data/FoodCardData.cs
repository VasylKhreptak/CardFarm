using UniRx;

namespace Cards.Data
{
    public class FoodCardData : SellableCardData
    {
        public IntReactiveProperty NutritionalValue = new IntReactiveProperty();
        public IntReactiveProperty MaxNutritionalValue = new IntReactiveProperty();
    }
}
