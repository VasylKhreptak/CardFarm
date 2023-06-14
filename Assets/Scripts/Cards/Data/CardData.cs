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
        public ReactiveProperty<CardData> UpperCard = new ReactiveProperty<CardData>();
        public ReactiveProperty<CardData> BottomCard = new ReactiveProperty<CardData>();
        public ReactiveCollection<CardData> UpperCards = new ReactiveCollection<CardData>();
        public ReactiveCollection<CardData> BottomCards = new ReactiveCollection<CardData>();
        public UpperCardsProvider UpperCardsProvider;
        public BottomCardsProvider BottomCardsProvider;
        public Transform BottomCardFollowPoint;
    }
}
