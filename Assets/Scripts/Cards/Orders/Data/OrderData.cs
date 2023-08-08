using System.Collections.Generic;
using Cards.Core;
using Cards.Data;
using ScriptableObjects.Scripts.Cards.Recipes;
using UniRx;
using UnityEngine;

namespace Cards.Orders.Data
{
    public class OrderData : CardData
    {
        public ReactiveProperty<Sprite> OrderIcon = new ReactiveProperty<Sprite>();
        public Card OrderRequiredCard;
        public IntReactiveProperty TargetCardsCount = new IntReactiveProperty();
        public IntReactiveProperty CurrentCardsCount = new IntReactiveProperty();
        public IntReactiveProperty LeftCardsCount = new IntReactiveProperty();
        public List<CardWeight> Rewards = new List<CardWeight>();
        public BoolReactiveProperty IsOrderCompleted = new BoolReactiveProperty();
    }
}
