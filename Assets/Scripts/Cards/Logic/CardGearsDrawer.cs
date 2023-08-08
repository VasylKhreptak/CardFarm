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
        [SerializeField] private CardDataHolder _cardData;

        [Header("Preferences")]
        [SerializeField] private float _height = 2f;

        private CompositeDisposable _subscriptions = new CompositeDisposable();
        private IDisposable _positionUpdateSubscription;

        private GameObject _gearsObject;

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
            _cardData = GetComponentInParent<CardDataHolder>(true);
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
            _gearsObject = _cardTablePooler.Spawn(CardTablePool.RotatingGears);
            _gearsObject.transform.localRotation = Quaternion.identity;

            _positionUpdateSubscription = _cardData
                .GroupCenter
                .DoOnSubscribe(() =>
                {
                    RenderGearsOnTop();
                    UpdateGearPosition();
                })
                .Subscribe(_ =>
                {
                    RenderGearsOnTop();
                    UpdateGearPosition();
                });
        }

        private void UpdateGearPosition()
        {
            if (_gearsObject == null) return;

            Vector3 position = _cardData.GroupCenter.Value;
            position.y = _height;

            _gearsObject.transform.position = position;
        }

        private void RenderGearsOnTop()
        {
            if (_gearsObject == null) return;

            _gearsObject.transform.SetAsLastSibling();
        }

        private void StopDrawingGears()
        {
            if (_gearsObject != null)
            {
                _gearsObject.SetActive(false);
                _gearsObject = null;
            }

            _positionUpdateSubscription?.Dispose();
        }
    }
}
