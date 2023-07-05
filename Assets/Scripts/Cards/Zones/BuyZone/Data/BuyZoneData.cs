using Cards.Core;
using Cards.Zones.Data;
using UniRx;
using UnityEngine;

namespace Cards.Zones.BuyZone.Data
{
    public class BuyZoneData : ZoneData
    {
        public ReactiveProperty<Card> TargetCard = new ReactiveProperty<Card>();
        public IntReactiveProperty Price = new IntReactiveProperty(3);
        public IntReactiveProperty CollectedCoins = new IntReactiveProperty(0);
        public IntReactiveProperty LeftCoins = new IntReactiveProperty(3);

        public Transform BoughtCardSpawnPoint;

        public BuyZoneCallbacks BuyZoneCallbacks = new BuyZoneCallbacks();
    }
}
