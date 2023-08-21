using System;
using System.Collections.Generic;
using Cards.Data;
using Constraints.CardTable;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Cards.Logic.Updaters
{
    public class CardPusher : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        [FormerlySerializedAs("_pushSpeedAmplifier")]
        [Header("Preferences")]
        [SerializeField] private float _pushSpeed = 1f;

        private IDisposable _pushSubscription;
        private CompositeDisposable _subscriptions = new CompositeDisposable();

        private CardsTableBounds _cardsTableBounds;

        [Inject]
        private void Constructor(CardsTableBounds cardsTableBounds)
        {
            _cardsTableBounds = cardsTableBounds;
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
        }

        #endregion

        private void StartObserving()
        {
            _cardData.IsAnyGroupCardSelected.Subscribe(_ => OnCardEnvironmentChanged()).AddTo(_subscriptions);
            _cardData.IsPlayingAnyAnimation.Subscribe(_ => OnCardEnvironmentChanged()).AddTo(_subscriptions);
            _cardData.IsOverlayed.Subscribe(_ => OnCardEnvironmentChanged()).AddTo(_subscriptions);
        }

        private void StopObserving()
        {
            _subscriptions.Clear();
            StopPushing();
        }

        private void OnCardEnvironmentChanged()
        {
            bool canPush;

            bool isAnyGroupCardSelected = _cardData.IsAnyGroupCardSelected.Value;
            bool isPlayingAnyAnimation = _cardData.IsPlayingAnyAnimation.Value;
            bool isOverlayed = _cardData.IsOverlayed.Value;

            canPush =
                isAnyGroupCardSelected == false
                && isPlayingAnyAnimation == false
                && isOverlayed == false;

            if (canPush)
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

            _pushSubscription = Observable.EveryUpdate().Subscribe(_ => PushCardStep());
        }

        private void StopPushing()
        {
            _pushSubscription?.Dispose();
        }

        private void PushCardStep()
        {
            if (_cardData.OverlappingCards.Count == 0) return;

            Vector3 position = _cardData.transform.position;
            Vector3 direction = Vector3.zero;
            List<CardData> overlappingCards = _cardData.OverlappingCards;

            for (int i = 0; i < overlappingCards.Count; i++)
            {
                CardData overlappingCard = overlappingCards[i];

                Vector3 directionToCard = overlappingCard.transform.position - _cardData.transform.position;

                directionToCard.Normalize();

                direction += directionToCard;
            }

            direction.y = 0;
            direction.Normalize();

            direction *= _pushSpeed;

            position -= direction * Time.deltaTime;

            position = _cardsTableBounds.Clamp(_cardData.RectTransform, position);

            _cardData.transform.position = position;
        }
    }
}
