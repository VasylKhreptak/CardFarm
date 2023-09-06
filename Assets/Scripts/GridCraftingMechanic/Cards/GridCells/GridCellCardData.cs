using System;
using Cards.Core;
using Cards.Data;
using UniRx;
using UnityEngine;

namespace GridCraftingMechanic.Cards.GridCells
{
    public class GridCellCardData : CardData
    {
        [Header("Grid Cell Data")]
        public ReactiveProperty<Card> TargetCard = new ReactiveProperty<Card>();
        public BoolReactiveProperty ContainsTargetCard = new BoolReactiveProperty(false);

        public BoolReactiveProperty IsUnlocked = new BoolReactiveProperty(false);
    }
}
