using UniRx;

namespace Cards.Data
{
    public class FoodCardData : SellableCardData
    {
        public IntReactiveProperty NutritionalValue = new IntReactiveProperty();
    }
}
