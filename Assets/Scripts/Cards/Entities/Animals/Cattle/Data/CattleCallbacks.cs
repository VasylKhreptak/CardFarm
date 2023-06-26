using System;
using Cards.Core;

namespace Cards.Entities.Animals.Cattle.Data
{
    [Serializable]
    public class CattleCallbacks
    {
        public Action<Card> OnItemSpawned;
        public Action OnItemSpawnedNoArgs;
    }
}
