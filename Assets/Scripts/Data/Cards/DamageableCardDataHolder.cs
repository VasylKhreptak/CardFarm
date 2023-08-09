using System;
using Data.Cards.Core;

namespace Data.Cards
{
    [Serializable]
    public class DamageableCardDataHolder : CardDataHolder
    {
        public int Health;

        public DamageableCardDataHolder()
        {

        }

        public DamageableCardDataHolder(CardDataHolder cardDataHolder) : base(cardDataHolder)
        {

        }

        public DamageableCardDataHolder(DamageableCardDataHolder damageableCardDataHolder) : base(damageableCardDataHolder)
        {
            Health = damageableCardDataHolder.Health;
        }
    }
}
