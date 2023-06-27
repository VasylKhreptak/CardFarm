using System;
using Cards.Core;
using Cards.Zones.Data;
using UniRx;
using UnityEngine;

namespace Cards.Zones.BoosterBuyZone.Data
{
    public class BoosterBuyZoneData : ZoneData
    {
        [Header("Preferences")]
        [SerializeField] private Card _targetBoosterCard = Core.Card.HumbleBeginningsBooster;

        public Card TargetBoosterCard => _targetBoosterCard;

        public IntReactiveProperty BoosterPrice = new IntReactiveProperty(3);

        public Transform BoosterSpawnPoint;

        public Action onSpawnedBooster;
    }
}
