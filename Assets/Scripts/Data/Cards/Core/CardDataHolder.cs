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
        public Color StatsIconColor = Color.white;
        public Sprite Icon;

        public string Description;

        public CardDataHolder()
        {
            
        }
        
        public CardDataHolder(CardDataHolder cardDataHolder)
        {
            Name = cardDataHolder.Name;
            NameColor = cardDataHolder.NameColor;
            BodyColor = cardDataHolder.BodyColor;
            HeaderColor = cardDataHolder.HeaderColor;
            StatsIconColor = cardDataHolder.StatsIconColor;
            Icon = cardDataHolder.Icon;
            Description = cardDataHolder.Description;
        }
    }
}
