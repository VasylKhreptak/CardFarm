using System.Collections.Generic;
using Cards.Core;
using Extensions.UniRx.UnityEngineBridge.Triggers;
using ScriptableObjects.Scripts.Cards.Recipes;
using UniRx;
using UnityEngine;

namespace Cards.Data
{
    public class CardData : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private float _baseHeight = 0.01f;
        [SerializeField] private float _heightOffset = 0.01f;
        [Space]
        [SerializeField] private bool _isWorker;

        public float BaseHeight => _baseHeight;
        public float HeightOffset => _heightOffset;

        public bool IsWorker => _isWorker;

        [Space]
        public int ID = -1;
        public IntReactiveProperty GroupID = new IntReactiveProperty(-1);

        public FloatReactiveProperty Height = new FloatReactiveProperty();

        public ObservableMouseTrigger MouseTrigger;

        public ReactiveProperty<Card> Card = new ReactiveProperty<Card>();
        public StringReactiveProperty Name = new StringReactiveProperty("Name");
        public ColorReactiveProperty NameColor = new ColorReactiveProperty(Color.white);
        public ReactiveProperty<Sprite> Background = new ReactiveProperty<Sprite>();
        public ReactiveProperty<Sprite> Icon = new ReactiveProperty<Sprite>();

        public ReactiveProperty<CardData> TopCard = new ReactiveProperty<CardData>();
        public ReactiveProperty<CardData> UpperCard = new ReactiveProperty<CardData>();
        public ReactiveProperty<CardData> BottomCard = new ReactiveProperty<CardData>();
        public ReactiveProperty<CardData> LowestCard = new ReactiveProperty<CardData>();
        public ReactiveProperty<CardData> LowestGroupCard = new ReactiveProperty<CardData>();
        public ReactiveProperty<CardData> JoinableCard = new ReactiveProperty<CardData>();

        public List<CardData> UpperCards = new List<CardData>();
        public List<CardData> BottomCards = new List<CardData>();
        public List<CardData> GroupCards = new List<CardData>();

        public Transform BottomCardFollowPoint;

        public BoolReactiveProperty IsTopCard = new BoolReactiveProperty();
        public BoolReactiveProperty IsBottomCard = new BoolReactiveProperty();
        public BoolReactiveProperty IsMiddleCard = new BoolReactiveProperty();
        public BoolReactiveProperty IsSingleCard = new BoolReactiveProperty();
        public BoolReactiveProperty IsSelectedCard = new BoolReactiveProperty();
        public BoolReactiveProperty IsLowestGroupCard = new BoolReactiveProperty();
        public BoolReactiveProperty CanBeUnderSelectedCard = new BoolReactiveProperty();
        public BoolReactiveProperty IsCompatibleWithSelectedCard = new BoolReactiveProperty();

        public ReactiveProperty<CardRecipe> CurrentRecipe = new ReactiveProperty<CardRecipe>();

        public CardDataCallbacks Callbacks = new CardDataCallbacks();
    }
}
