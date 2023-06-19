using Cards.Zones.Data;
using UniRx;

namespace Cards.Zones.BoosterBuyZone.Data
{
    public class BoosterBuyZoneData : ZoneData
    {
        public IntReactiveProperty BoosterPrice = new IntReactiveProperty(3);
    }
}
