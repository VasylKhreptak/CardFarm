using System;
using UnityEngine;

namespace Data.Cards.Core
{
    [Serializable]
    public class CardDataHolder
    {
        public bool IsWorker;
        public bool IsFood;
        public bool IsResourceNode;
        public bool IsSellableCard;
        public bool IsAutomatedFactory;
        public bool IsAnimal;
        public bool IsDamageable;
        public bool IsOrder;
        public bool IsResource;
        public bool IsZone;
        public bool HasHeaderBackground = true;

        public string Name;
        public Color NameColor = Color.white;
        public Color BodyColor = Color.white;
        public Color HeaderColor = Color.white;
        public Color StatsTextColor = Color.white;
        public Sprite Icon;

        public string Description;

        public CardDataHolder()
        {

        }

        public CardDataHolder(CardDataHolder cardDataHolder)
        {
            IsWorker = cardDataHolder.IsWorker;
            IsFood = cardDataHolder.IsFood;
            IsResourceNode = cardDataHolder.IsResourceNode;
            IsSellableCard = cardDataHolder.IsSellableCard;
            IsAutomatedFactory = cardDataHolder.IsAutomatedFactory;
            IsAnimal = cardDataHolder.IsAnimal;
            IsDamageable = cardDataHolder.IsDamageable;
            IsOrder = cardDataHolder.IsOrder;
            IsResource = cardDataHolder.IsResource;
            IsZone = cardDataHolder.IsZone;
            HasHeaderBackground = cardDataHolder.HasHeaderBackground;

            Name = cardDataHolder.Name;
            NameColor = cardDataHolder.NameColor;
            BodyColor = cardDataHolder.BodyColor;
            HeaderColor = cardDataHolder.HeaderColor;
            Icon = cardDataHolder.Icon;
            Description = cardDataHolder.Description;
            StatsTextColor = cardDataHolder.StatsTextColor;
        }
    }
}
