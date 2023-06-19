﻿using Cards.Zones.Data;
using UniRx;
using UnityEngine;

namespace Cards.Zones.BoosterBuyZone.Data
{
    public class BoosterBuyZoneData : ZoneData
    {
        public IntReactiveProperty BoosterPrice = new IntReactiveProperty(3);

        public Transform BoosterSpawnPoint;
    }
}