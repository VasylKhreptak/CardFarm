using System;
using UnityEngine;
using Color = System.Drawing.Color;

namespace Data.Cards.Core
{
    [Serializable]
    public class CardDataHolder
    {
        public string Name;
        public Color NameColor;
        public Color BodyColor;
        public Color HeaderColor;
        public Color DataIconsColor;
        public Sprite Icon;
    }
}
