using System;
using Cards.Data;
using Constraints.CardTable;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Logic
{
    public class BottomCardPusher : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        [Header("Preferences")]
        [SerializeField] private float _speed = 3f;

        private CompositeDisposable _subscriptions = new CompositeDisposable();
        private IDisposable _pushSubscription;

        private CardsTableBounds _cardTableBounds;

        [Inject]
        private void Constructor(CardsTableBounds cardTableBounds)
        {
            _cardTableBounds = cardTableBounds;
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
            StartObserving();
        }

        private void OnDisable()
        {
            StopObserving();
            StopPushing();
        }

        #endregion

        private void StartObserving()
        {
            StopObserving();

            _cardData.IsPushedByAnyBottomCard.Subscribe(_ => OnDataChanged()).AddTo(_subscriptions);
        }

        private void StopObserving()
        {
            _subscriptions?.Clear();
        }

        private void OnDataChanged()
        {
            if (_cardData.IsPushedByAnyBottomCard.Value)
            {
                StartPushing();
            }
            else
            {
                StopPushing();
            }
        }

        private void StartPushing()
        {
            StopPushing();

            _pushSubscription = Observable
                .EveryUpdate()
                .Subscribe(_ => PushStep())
                .AddTo(_subscriptions);
        }

        private void StopPushing()
        {
            _pushSubscription?.Dispose();
        }

        private void PushStep()
        {
            Vector3 position = transform.position;

            position.z += _speed * Time.deltaTime;

            position = _cardTableBounds.Clamp(_cardData.RectTransform, position);

            _cardData.RectTransform.position = position;
        }
    }
}
