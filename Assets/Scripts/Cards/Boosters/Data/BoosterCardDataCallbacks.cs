using System;
using Cards.Core;

namespace Cards.Boosters.Data
{
    public class BoosterCardDataCallbacks
    {
        public Action OnClick;
        public Action<Card> OnSpawnedCard;
        public Action OnEmptied;
        public Action<int> OnRefilled;
    }
}
