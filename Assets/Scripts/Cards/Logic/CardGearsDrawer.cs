using System;
using Cards.Data;
using Cards.Factories.Data;
using CardsTable.PoolLogic;
using DG.Tweening;
using Graphics.VisualElements.Gears;
using ScriptableObjects.Scripts.Cards.AutomatedFactories.Recipes;
using ScriptableObjects.Scripts.Cards.Recipes;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Logic
{
    public class CardGearsDrawer : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        [Header("Preferences")]
        [SerializeField] private float _height = 2f;
        [SerializeField] private Color _spawnedRecipeResultProgressColor = Color.yellow;
        [SerializeField] private Color _defaultProgressColor;
        [SerializeField] private float _showDuration = 0.7f;

        private CompositeDisposable _subscriptions = new CompositeDisposable();
        private IDisposable _positionUpdateSubscription;
        private IDisposable _showDelaySubscription;

        private ReactiveProperty<GearsData> _gears = new ReactiveProperty<GearsData>();

        private FactoryData _factoryData;

        public IReadOnlyReactiveProperty<GearsData> Gears => _gears;

        public event Action OnDrawnCheckmark;

        private CardTablePooler _cardTablePooler;

        [Inject]
        private void Constructor(CardTablePooler cardTablePooler)
        {
            _cardTablePooler = cardTablePooler;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _cardData = GetComponentInParent<CardData>(true);
        }

        private void Awake()
        {
            if (_cardData.IsAutomatedFactory)
            {
                _factoryData = _cardData as FactoryData;
            }
        }

        private void OnEnable()
        {
            StartObservingCardData();

            _cardData.Callbacks.onExecutedRecipe += OnExecutedRecipe;

            if (_factoryData != null)
            {
                _factoryData.AutomatedFactoryCallbacks.onExecutedRecipe += OnExecutedFactoryRecipe;
            }
        }

        private void OnDisable()
        {
            StopObservingCardData();
            StopDrawingGears();
            SetProgressUpdatersState(true);

            _cardData.Callbacks.onExecutedRecipe -= OnExecutedRecipe;

            if (_factoryData != null)
            {
                _factoryData.AutomatedFactoryCallbacks.onExecutedRecipe -= OnExecutedFactoryRecipe;
            }

            _showDelaySubscription?.Dispose();
        }

        #endregion

        private void StartObservingCardData()
        {
            StopObservingCardData();

            _cardData.IsAnyGroupCardSelected.Subscribe(_ => OnCardDataUpdated()).AddTo(_subscriptions);
            _cardData.IsExecutingAnyRecipe.Subscribe(_ => OnCardDataUpdated()).AddTo(_subscriptions);
        }

        private void StopObservingCardData()
        {
            _subscriptions?.Clear();
        }

        private void OnExecutedFactoryRecipe(FactoryRecipe factoryRecipe) => OnExecutedAnyRecipe();

        private void OnExecutedRecipe(CardRecipe cardRecipe) => OnExecutedAnyRecipe();

        private void OnExecutedAnyRecipe()
        {
            _showDelaySubscription?.Dispose();
            GearsData gears = _gears.Value;

            if (gears == null) return;

            gears.ShowAnimation.Stop();
            gears.HideAnimation.Stop();
            gears.GearsShowAnimation.Stop();
            gears.GearsHideAnimation.Stop();
            gears.MarkShowAnimation.Stop();
            gears.MarkHideAnimation.Stop();

            StopObservingCardData();

            SetProgressUpdatersState(false);
            SetGroupCardsInteractableState(false);

            gears.CircularProgress.SetProgress(1f);
            gears.CircularProgress.SetColor(_spawnedRecipeResultProgressColor);

            gears.ShowAnimation.PlayForwardImmediate();
            gears.GearsHideAnimation.PlayForwardImmediate();

            gears.MarkShowAnimation.InitForward();
            gears.MarkShowAnimation.Animation.OnComplete(() =>
            {
                _showDelaySubscription?.Dispose();
                _showDelaySubscription = Observable.Timer(TimeSpan.FromSeconds(_showDuration)).Subscribe(_ =>
                {
                    SetGroupCardsInteractableState(true);
                    OnDrawnCheckmark?.Invoke();
                    gears.HideAnimation.InitForward();
                    gears.HideAnimation.Animation.OnComplete(() =>
                    {
                        StartObservingCardData();
                        SetProgressUpdatersState(true);
                    });
                    gears.HideAnimation.PlayCurrentAnimation();
                });
            });
            gears.MarkShowAnimation.PlayCurrentAnimation();
        }

        private void OnCardDataUpdated()
        {
            bool isAnyGroupCardSelected = _cardData.IsAnyGroupCardSelected.Value;
            bool isExecutingAnyRecipe = _cardData.IsExecutingAnyRecipe.Value;

            bool canDraw = isExecutingAnyRecipe && isAnyGroupCardSelected == false;

            if (canDraw)
            {
                StartDrawingGears();
            }
            else
            {
                StopDrawingGears();
            }
        }

        private void StartDrawingGears()
        {
            StopDrawingGears();

            SetProgressUpdatersState(true);

            if (_gears.Value == null)
            {
                GameObject gearsObject = _cardTablePooler.Spawn(CardTablePool.RotatingGears);
                _gears.Value = gearsObject.GetComponent<GearsData>();
            }

            _gears.Value.HideAnimation.Stop();
            _gears.Value.ShowAnimation.PlayForwardImmediate();
            _gears.Value.CircularProgress.SetColor(_defaultProgressColor);
            _gears.Value.GearsShowAnimation.MoveToEndState();
            _gears.Value.MarkHideAnimation.MoveToEndState();

            _gears.Value.transform.localRotation = Quaternion.identity;

            _positionUpdateSubscription = _cardData
                .GroupCenter
                .DoOnSubscribe(() =>
                {
                    UpdateGearsSortingLayer();
                    UpdateGearPosition();
                })
                .Subscribe(_ =>
                {
                    UpdateGearsSortingLayer();
                    UpdateGearPosition();
                });
        }

        private void UpdateGearPosition()
        {
            if (_gears.Value == null) return;

            Vector3 position = _cardData.GroupCenter.Value;
            position.y = _height;

            _gears.Value.transform.position = position;
        }

        private void UpdateGearsSortingLayer()
        {
            if (_gears.Value == null) return;

            CardData lastGroupCard = _cardData.LastGroupCard.Value;

            if (lastGroupCard == null)
            {
                _gears.Value.transform.SetAsLastSibling();
            }
            else
            {
                _gears.Value.transform.SetSiblingIndex(lastGroupCard.transform.GetSiblingIndex() + 1);
            }
        }

        private void StopDrawingGears()
        {
            _positionUpdateSubscription?.Dispose();

            if (_gears.Value == null) return;

            GameObject gears = _gears.Value.gameObject;
            _gears.Value.ShowAnimation.Stop();
            _gears.Value.HideAnimation.InitForward();
            _gears.Value.HideAnimation.Animation
                .OnPlay(() =>
                {
                    UpdateGearPosition();
                    UpdateGearsSortingLayer();
                })
                .OnUpdate(() =>
                {
                    UpdateGearPosition();
                    UpdateGearsSortingLayer();
                })
                .OnComplete(() =>
                {
                    _gears.Value = null;
                    gears.SetActive(false);
                });

            _gears.Value.HideAnimation.PlayCurrentAnimation();
        }

        private void SetProgressUpdatersState(bool enabled)
        {
            foreach (var progressUpdater in _cardData.CardProgressUpdaters)
            {
                progressUpdater.enabled = enabled;
            }
        }

        private void SetGroupCardsInteractableState(bool isInteractable)
        {
            foreach (var groupCard in _cardData.GroupCards)
            {
                groupCard.IsInteractable.Value = isInteractable;
            }
        }
    }
}
