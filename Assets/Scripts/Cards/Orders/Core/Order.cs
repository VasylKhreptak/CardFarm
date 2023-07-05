using System;
using Cards.Core;
using UnityEngine;

namespace Cards.Orders.Core
{
    [Serializable]
    public class Order
    {
        public Sprite Icon;
        public Card TargetCard;
        public Card RewardCard;
    }
}
