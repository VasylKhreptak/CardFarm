using System;
using Cards.Data;
using CardsTable.PoolLogic;
using DG.Tweening;
using Graphics.VisualElements.Gears;
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

        private CompositeDisposable _subscriptions = new CompositeDisposable();
        private IDisposable _positionUpdateSubscription;

        private ReactiveProperty<GearsData> _gears = new ReactiveProperty<GearsData>();

        public IReadOnlyReactiveProperty<GearsData> Gears => _gears;

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

        private void OnEnable()
        {
            StartObservingCardData();
        }

        private void OnDisable()
        {
            StopObservingCardData();
            StopDrawingGears();
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

            if (_gears.Value == null)
            {
                GameObject gearsObject = _cardTablePooler.Spawn(CardTablePool.RotatingGears);
                _gears.Value = gearsObject.GetComponent<GearsData>();
            }

            _gears.Value.HideAnimation.Stop();
            _gears.Value.ShowAnimation.PlayForwardImmediate();

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
    }
}
