using System;
using Cards.Core;
using Cards.Zones.Data;
using Graphics.Animations.Core;
using UniRx;
using UnityEngine;

namespace Cards.Zones.SellZone.Data
{
    public class SellZoneData : ZoneData
    {
        [Header("Preferences")]
        [SerializeField] private float _coinSpawnInterval = 0.1f;

        public float CoinSpawnInterval => _coinSpawnInterval;

        public Transform CoinSpawnPoint;

        public Action<Card> onSoldCard;

        public IntReactiveProperty SelectedCardsTotalPrice = new IntReactiveProperty(0);

        public JumpFlipAnimation CoinFlipAnimation;
    }
}
