using UniRx;

namespace Cards.Data
{
    public class DamageableCardData : CardDataHolder
    {
        public IntReactiveProperty MaxHealth = new IntReactiveProperty(5);
        public IntReactiveProperty Health = new IntReactiveProperty(5);
    }
}
