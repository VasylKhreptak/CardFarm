using System;
using UniRx;

namespace Cards.Data
{
    [Serializable]
    public class DamageableCardData : CardData
    {
        public IntReactiveProperty MaxHealth = new IntReactiveProperty(5);
        public IntReactiveProperty Health = new IntReactiveProperty(5);
    }
}
