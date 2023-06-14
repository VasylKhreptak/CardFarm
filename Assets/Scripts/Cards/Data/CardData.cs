using System.Collections.Generic;
using Cards.Core;
using Cards.Logic;
using Extensions.UniRx.UnityEngineBridge.Triggers;
using UniRx;
using UnityEngine;

namespace Cards.Data
{
    public class CardData : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private float _baseHeight = 0.001f;
        [SerializeField] private float _heightOffset = 0.001f;

        public float BaseHeight => _baseHeight;
        public float HeightOffset => _heightOffset;

        public int ID = -1;
        public FloatReactiveProperty Height = new FloatReactiveProperty();
        public ObservableMouseTrigger MouseTrigger;

        public ReactiveProperty<CardType> CardType = new ReactiveProperty<CardType>();
        public StringReactiveProperty Name = new StringReactiveProperty("Name");
        public ReactiveProperty<Sprite> Background = new ReactiveProperty<Sprite>();
        public ReactiveProperty<Sprite> Icon = new ReactiveProperty<Sprite>();

        public ReactiveProperty<CardData> TopCard = new ReactiveProperty<CardData>();
        public ReactiveProperty<CardData> UpperCard = new ReactiveProperty<CardData>();
        public ReactiveProperty<CardData> BottomCard = new ReactiveProperty<CardData>();
        public ReactiveProperty<CardData> LowestCard = new ReactiveProperty<CardData>();

        public List<CardData> UpperCards = new List<CardData>();
        public List<CardData> BottomCards = new List<CardData>();
        public List<CardData> CardsGroup = new List<CardData>();

        public UpperCardsProvider UpperCardsProvider;
        public BottomCardsProvider BottomCardsProvider;

        public Transform BottomCardFollowPoint;

        public BoolReactiveProperty IsTopCard = new BoolReactiveProperty();
        public BoolReactiveProperty IsBottomCard = new BoolReactiveProperty();
        public BoolReactiveProperty IsMiddleCard = new BoolReactiveProperty();
        public BoolReactiveProperty IsSingleCard = new BoolReactiveProperty();

        public CardDataCallbacks Callbacks = new CardDataCallbacks();
    }
}
