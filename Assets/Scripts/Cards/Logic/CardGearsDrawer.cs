using System;
using Cards.Data;
using CardsTable.PoolLogic;
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

        private ReactiveProperty<GameObject> _gearsObject = new ReactiveProperty<GameObject>();

        public IReadOnlyReactiveProperty<GameObject> GearsObject => _gearsObject;

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
            _gearsObject.Value = _cardTablePooler.Spawn(CardTablePool.RotatingGears);
            GameObject gearsObject = _gearsObject.Value;

            gearsObject.transform.localRotation = Quaternion.identity;

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
            if (_gearsObject == null) return;

            Vector3 position = _cardData.GroupCenter.Value;
            position.y = _height;

            _gearsObject.Value.transform.position = position;
        }

        private void UpdateGearsSortingLayer()
        {
            if (_gearsObject == null) return;

            CardData lastGroupCard = _cardData.LastGroupCard.Value;
            
            if (lastGroupCard == null)
            {
                _gearsObject.Value.transform.SetAsLastSibling();
            }
            else
            {
                _gearsObject.Value.transform.SetSiblingIndex(lastGroupCard.transform.GetSiblingIndex() + 1);
            }
        }

        private void StopDrawingGears()
        {
            if (_gearsObject.Value != null)
            {
                _gearsObject.Value.SetActive(false);
                _gearsObject.Value = null;
            }

            _positionUpdateSubscription?.Dispose();
        }
    }
}
