using System;
using UnityEngine;

namespace Data.Cards.Core
{
    [Serializable]
    public class CardDataHolder
    {
        public string Name;
        public Color NameColor = Color.white;
        public Color BodyColor = Color.white;
        public Color HeaderColor = Color.white;
        public Color DataIconsColor = Color.white;
        public Sprite Icon;
    }
}
