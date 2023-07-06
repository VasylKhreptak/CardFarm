using Cards.Graphics.VisualElements;
using Cards.Zones.BuyZone.Data;

namespace Cards.Zones.BuyZone.Graphics.VisualElements
{
    public class BuyZoneNameText : CardNameText
    {
        protected override void SetName(string name)
        {
            BuyZoneData buyZoneData = _cardData as BuyZoneData;

            if (buyZoneData.IsLocked.Value) return;

            base.SetName(name);
        }
    }
}
