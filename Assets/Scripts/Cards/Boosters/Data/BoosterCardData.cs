using System;
using Cards.Data;
using UniRx;

namespace Cards.Boosters.Data
{
    public class BoosterCardData : CardData
    {
        public Action OnClick;

        public IntReactiveProperty TotalCards = new IntReactiveProperty();
        public IntReactiveProperty LeftCards = new IntReactiveProperty();
    }
}
