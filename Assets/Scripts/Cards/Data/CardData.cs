using System.Collections.Generic;
using Cards.Core;
using Extensions.UniRx.UnityEngineBridge.Triggers;
using ScriptableObjects.Scripts.Cards.Recipes;
using ScriptableObjects.Scripts.Cards.ReproductionRecipes;
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
        [SerializeField] private bool _isFood;
        [SerializeField] private bool _isResourceNode;
        [SerializeField] private bool _isSellableCard;
        [SerializeField] private bool _canSortingLayerChange = true;
        [SerializeField] private bool _isAutomatedFactory;
        [SerializeField] private bool _isIncubator;
        [SerializeField] private bool _isAnimal;
        [SerializeField] private bool _isStackable = true;

        public float BaseHeight => _baseHeight;
        public float HeightOffset => _heightOffset;

        public bool IsWorker => _isWorker;
        public bool IsSellableCard => _isSellableCard;
        public bool CanSortingLayerChange => _canSortingLayerChange;
        public bool IsAutomatedFactory => _isAutomatedFactory;
        public bool IsIncubator => _isIncubator;
        public bool IsFood => _isFood;
        public bool IsResourceNode => _isResourceNode;
        public bool IsAnimal => _isAnimal;
        public bool IsStackable => _isStackable;

        [Space]
        public int ID = -1;
        public IntReactiveProperty GroupID = new IntReactiveProperty(-1);

        public FloatReactiveProperty Height = new FloatReactiveProperty();

        public ObservableMouseTrigger MouseTrigger;

        public Collider Collider;

        public ReactiveProperty<Card> Card = new ReactiveProperty<Card>();
        public StringReactiveProperty Name = new StringReactiveProperty("Name");
        public ColorReactiveProperty NameColor = new ColorReactiveProperty(Color.white);
        public ReactiveProperty<Sprite> Background = new ReactiveProperty<Sprite>();
        public ReactiveProperty<Sprite> Icon = new ReactiveProperty<Sprite>();

        public BoolReactiveProperty IsInteractable = new BoolReactiveProperty(true);

        public ReactiveProperty<CardData> UpperCard = new ReactiveProperty<CardData>();
        public ReactiveProperty<CardData> BottomCard = new ReactiveProperty<CardData>();

        public ReactiveProperty<CardData> FirstUpperCard = new ReactiveProperty<CardData>();
        public ReactiveProperty<CardData> LastBottomCard = new ReactiveProperty<CardData>();

        public ReactiveProperty<CardData> FirstGroupCard = new ReactiveProperty<CardData>();
        public ReactiveProperty<CardData> LastGroupCard = new ReactiveProperty<CardData>();

        public ReactiveProperty<CardData> JoinableCard = new ReactiveProperty<CardData>();

        public List<CardData> UpperCards = new List<CardData>();
        public List<CardData> BottomCards = new List<CardData>();
        public List<CardData> GroupCards = new List<CardData>();

        public Transform BottomCardFollowPoint;

        public BoolReactiveProperty IsTopCard = new BoolReactiveProperty();
        public BoolReactiveProperty IsBottomCard = new BoolReactiveProperty();
        public BoolReactiveProperty IsMiddleCard = new BoolReactiveProperty();
        public BoolReactiveProperty IsSingleCard = new BoolReactiveProperty();
        public BoolReactiveProperty IsSelected = new BoolReactiveProperty();
        public BoolReactiveProperty IsLowestGroupCard = new BoolReactiveProperty();
        public BoolReactiveProperty CanBeUnderSelectedCard = new BoolReactiveProperty();
        public BoolReactiveProperty IsCompatibleWithSelectedCard = new BoolReactiveProperty();
        public BoolReactiveProperty CanBeSelected = new BoolReactiveProperty();

        public BoolReactiveProperty IsInsideCardsTable = new BoolReactiveProperty();
        public Vector3ReactiveProperty ClampedPosition = new Vector3ReactiveProperty();
        public BoolReactiveProperty IsTakingPartInRecipe = new BoolReactiveProperty();

        public ReactiveProperty<CardRecipe> CurrentRecipe = new ReactiveProperty<CardRecipe>(null);

        public IntReactiveProperty Durability = new IntReactiveProperty(1);
        public IntReactiveProperty MaxDurability = new IntReactiveProperty(1);

        public CardAnimations Animations = new CardAnimations();
        public BoolReactiveProperty IsPlayingAnyAnimation = new BoolReactiveProperty();

        public CardDataCallbacks Callbacks = new CardDataCallbacks();

        public ReactiveProperty<CardReproductionRecipe> CurrentReproductionRecipe = new ReactiveProperty<CardReproductionRecipe>();
    }
}
