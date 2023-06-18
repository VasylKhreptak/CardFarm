using System;
using Cards.Data;
using UnityEngine;

namespace Cards.Boosters.Data
{
    [DisallowMultipleComponent]
    public class BoosterCardData : CardData
    {
        public Action OnClick;
    }
}
