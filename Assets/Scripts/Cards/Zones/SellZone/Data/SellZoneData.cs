using System;
using Cards.Core;
using Cards.Zones.Data;
using UnityEngine;

namespace Cards.Zones.SellZone.Data
{
    public class SellZoneData : ZoneData
    {
        public Transform CoinSpawnPoint;

        public Action<Card> onSoldCard;
    }
}
