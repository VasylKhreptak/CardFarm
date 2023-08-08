using System;
using System.Collections.Generic;
using Cards.Core;
using Cards.Gestures.PositionShake;
using Cards.Graphics.Animations;
using Cards.Graphics.Outlines;
using Cards.Logic;
using Cards.Recipes;
using Extensions.UniRx.UnityEngineBridge.Triggers;
using ScriptableObjects.Scripts.Cards.Recipes;
using ScriptableObjects.Scripts.Cards.ReproductionRecipes;
using Tags.Cards;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Data
{
    public class CardDataHolder : MonoBehaviour, IValidatable
    {
        [SerializeField] private float _baseHeight = 0.01f;
        [SerializeField] private float _selectedHeight = 0.1f;
        [Space]
        [SerializeField] private bool _isWorker;
        [SerializeField] private bool _isFood;
        [SerializeField] private bool _isResourceNode;
        [SerializeField] private bool _isSellableCard;
        [SerializeField] private bool _canSortingLayerChange = true;
        [SerializeField] private bool _isAutomatedFactory;
        [SerializeField] private bool _isAnimal;
        [SerializeField] private bool _isStackable = true;
        [SerializeField] private bool _isDamageable = true;
        [SerializeField] private bool _canBeStackedOnlyWithSameCard;
        [SerializeField] private bool _isOrder;
        [SerializeField] private bool _isBreakable = true;
        [SerializeField] private bool _canBeUnderCards = true;
        [SerializeField] private bool _isResource;
        [SerializeField] private bool _canBePlacedInChest;
        [SerializeField] private bool _canBeStackedWithSameCard = true;
        [SerializeField] private bool _isZone;

        public float BaseHeight => _baseHeight;
        public float SelectedHeight => _selectedHeight;

        public bool IsWorker => _isWorker;
        public bool IsSellableCard => _isSellableCard;
        public bool CanSortingLayerChange => _canSortingLayerChange;
        public bool IsAutomatedFactory => _isAutomatedFactory;
        public bool IsFood => _isFood;
        public bool IsResourceNode => _isResourceNode;
        public bool IsAnimal => _isAnimal;
        public bool IsStackable => _isStackable;
        public bool IsDamageable => _isDamageable;
        public bool CanBeStackedOnlyWithSameCard => _canBeStackedOnlyWithSameCard;
        public bool IsOrder => _isOrder;
        public bool IsBreakable => _isBreakable;
        public bool CanBeUnderCards => _canBeUnderCards;
        public bool IsResource => _isResource;
        public bool CanBePlacedInChest => _canBePlacedInChest;
        public bool CanBeStackedWithSameCard => _canBeStackedWithSameCard;
        public bool IsZone => _isZone;

        public int ID = -1;
        public IntReactiveProperty GroupID = new IntReactiveProperty(-1);

        public FloatReactiveProperty Height = new FloatReactiveProperty();

        public ObservableMouseTrigger MouseTrigger;

        public Collider Collider;

        public RectTransform RectTransform;

        public ReactiveProperty<Card> Card = new ReactiveProperty<Card>();
        public StringReactiveProperty Name = new StringReactiveProperty("Name");
        public ColorReactiveProperty NameColor = new ColorReactiveProperty(Color.white);
        public ReactiveProperty<Sprite> Background = new ReactiveProperty<Sprite>();
        public ReactiveProperty<Sprite> Icon = new ReactiveProperty<Sprite>();

        public BoolReactiveProperty IsInteractable = new BoolReactiveProperty(true);

        public ReactiveProperty<CardDataHolder> UpperCard = new ReactiveProperty<CardDataHolder>();
        public ReactiveProperty<CardDataHolder> BottomCard = new ReactiveProperty<CardDataHolder>();

        public ReactiveProperty<CardDataHolder> FirstUpperCard = new ReactiveProperty<CardDataHolder>();
        public ReactiveProperty<CardDataHolder> LastBottomCard = new ReactiveProperty<CardDataHolder>();

        public ReactiveProperty<CardDataHolder> FirstGroupCard = new ReactiveProperty<CardDataHolder>();
        public ReactiveProperty<CardDataHolder> LastGroupCard = new ReactiveProperty<CardDataHolder>();

        public ReactiveProperty<CardDataHolder> JoinableCard = new ReactiveProperty<CardDataHolder>();

        public List<CardDataHolder> UpperCards = new List<CardDataHolder>();
        public List<CardDataHolder> BottomCards = new List<CardDataHolder>();
        public List<CardDataHolder> GroupCards = new List<CardDataHolder>();

        public Transform BottomCardFollowPoint;

        public BoolReactiveProperty IsTopCard = new BoolReactiveProperty();
        public BoolReactiveProperty IsBottomCard = new BoolReactiveProperty();
        public BoolReactiveProperty IsMiddleCard = new BoolReactiveProperty();
        public BoolReactiveProperty IsSingleCard = new BoolReactiveProperty();
        public BoolReactiveProperty IsSelected = new BoolReactiveProperty();
        public BoolReactiveProperty IsLastGroupCard = new BoolReactiveProperty();
        public BoolReactiveProperty CanBeUnderSelectedCard = new BoolReactiveProperty();
        public BoolReactiveProperty IsCompatibleWithSelectedCard = new BoolReactiveProperty();
        public BoolReactiveProperty CanBeSelected = new BoolReactiveProperty();
        public BoolReactiveProperty IsAnyGroupCardSelected = new BoolReactiveProperty();

        public BoolReactiveProperty IsInsideCardsTable = new BoolReactiveProperty();
        public BoolReactiveProperty IsTakingPartInRecipe = new BoolReactiveProperty();

        public RecipeExecutor RecipeExecutor;
        public ReactiveProperty<CardRecipe> CurrentRecipe = new ReactiveProperty<CardRecipe>(null);
        public List<CardRecipe> PossibleRecipes = new List<CardRecipe>();

        public BaseReproductionLogic ReproductionLogic;
        public ReactiveProperty<CardReproductionRecipe> CurrentReproductionRecipe = new ReactiveProperty<CardReproductionRecipe>();

        public IntReactiveProperty Durability = new IntReactiveProperty(1);
        public IntReactiveProperty MaxDurability = new IntReactiveProperty(1);

        public CardAnimations Animations = new CardAnimations();
        public BoolReactiveProperty IsPlayingAnyAnimation = new BoolReactiveProperty();

        public BoolReactiveProperty IsActivelyFollowingCard = new BoolReactiveProperty();

        public CardDataCallbacks Callbacks = new CardDataCallbacks();

        public PositionShakeObserver PositionShakeObserver;

        public CardSelectedHeightController CardSelectedHeightController;

        public List<CardDataHolder> OverlappingCards = new List<CardDataHolder>();

        public BoolReactiveProperty IsNew = new BoolReactiveProperty(false);

        public QuestOutline QuestOutline;

        public Vector3ReactiveProperty GroupCenter = new Vector3ReactiveProperty();
        
        public BoolReactiveProperty IsExecutingAnyRecipe = new BoolReactiveProperty();

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public virtual void Validate()
        {
            MouseTrigger = GetComponentInChildren<ObservableMouseTrigger>(true);

            BottomCardFollowPoint bottomCardFollowPoint = GetComponentInChildren<BottomCardFollowPoint>(true);
            BottomCardFollowPoint = bottomCardFollowPoint != null ? bottomCardFollowPoint.transform : null;

            Collider = GetComponentInChildren<Collider>();

            PositionShakeObserver = GetComponentInChildren<PositionShakeObserver>(true);

            Animations.MoveAnimation = GetComponentInChildren<CardMoveAnimation>(true);
            Animations.JumpAnimation = GetComponentInChildren<CardJumpAnimation>(true);
            Animations.FlipAnimation = GetComponentInChildren<CardFlipAnimation>(true);
            Animations.ShakeAnimation = GetComponentInChildren<CardShakeAnimation>(true);
            Animations.ContinuousJumpingAnimation = GetComponentInChildren<CardContinuousJumpingAnimation>(true);
            CardSelectedHeightController = GetComponentInChildren<CardSelectedHeightController>(true);

            RectTransform = GetComponentInChildren<RectTransform>(true);

            QuestOutline = GetComponentInChildren<QuestOutline>(true);

            RecipeExecutor = GetComponentInChildren<RecipeExecutor>(true);
            ReproductionLogic = GetComponentInChildren<BaseReproductionLogic>(true);
        }

        #endregion
    }
}