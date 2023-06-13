using System;
using UniRx;
using UnityEngine;

namespace Cards.Data
{
    [Serializable]
    public class CardData
    {
        public ReactiveProperty<CardType> CardType = new ReactiveProperty<CardType>();
        public StringReactiveProperty Name = new StringReactiveProperty();
        public ReactiveProperty<Sprite> Background = new ReactiveProperty<Sprite>();
        public ReactiveProperty<Sprite> Icon = new ReactiveProperty<Sprite>();
    }
}
