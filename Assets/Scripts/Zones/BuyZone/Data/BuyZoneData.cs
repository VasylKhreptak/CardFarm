using Cards.Core;
using UniRx;
using Zones.Data;

namespace Zones.BuyZone.Data
{
    public class BuyZoneData : ZoneData
    {
        public IntReactiveProperty Price = new IntReactiveProperty();
        public IntReactiveProperty CollectedCoinsCount = new IntReactiveProperty();
        public IntReactiveProperty LeftCoinsCount = new IntReactiveProperty();
        public Card TargetCard;

        public BuyZoneCallbacks Callbacks = new BuyZoneCallbacks();
    }
}
