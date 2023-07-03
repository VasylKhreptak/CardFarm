using System;
using Cards.Chests.SellableChest.Data;
using Cards.Core;
using Cards.Data;
using Extensions;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Chests.SellableChest.Logic
{
    public class ChestFeeder : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private ChestSellableCardData _cardData;

        [Header("Preferences")]
        [SerializeField] private Card _chestType;
        [SerializeField] private float _maxDistanceToFollowPoint = 0.5f;
        [SerializeField] private float _distanceCheckInterval = 0.2f;

        private IDisposable _bottomCardSubscription;
        private IDisposable _distanceCheckSubscription;

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _cardData = GetComponentInParent<ChestSellableCardData>(true);
        }

        private void OnEnable()
        {
            StartObservingBottomCard();
        }

        private void OnDisable()
        {
            StopObservingBottomCardDistance();
            StopObservingBottomCard();
        }

        #endregion

        private void StartObservingBottomCard()
        {
            StopObservingBottomCard();
            _bottomCardSubscription = _cardData.BottomCard.Subscribe(OnBottomCardUpdated);
        }

        private void StopObservingBottomCard()
        {
            _bottomCardSubscription?.Dispose();
        }

        private void OnBottomCardUpdated(CardData bottomCard)
        {
            StopObservingBottomCardDistance();

            if (bottomCard == null) return;

            if (bottomCard.Card.Value == _chestType)
            {
                StartObservingBottomCardDistance(bottomCard);
            }
            else
            {
                bottomCard.UnlinkFromUpper();
            }
        }

        private void StartObservingBottomCardDistance(CardData bottomCard)
        {
            StopObservingBottomCardDistance();
            _distanceCheckSubscription = Observable
                .Interval(TimeSpan.FromSeconds(_distanceCheckInterval))
                .DoOnSubscribe(CheckBottomCardDistance)
                .Subscribe(_ => CheckBottomCardDistance());
        }

        private void StopObservingBottomCardDistance()
        {
            _distanceCheckSubscription?.Dispose();
        }

        private void CheckBottomCardDistance()
        {
            CardData bottomCard = _cardData.BottomCard.Value;

            if (_cardData.BottomCard.Value == null)
            {
                StopObservingBottomCardDistance();
                return;
            }

            float distance = Vector3.Distance(_cardData.BottomCardFollowPoint.position, bottomCard.transform.position);

            if (distance < _maxDistanceToFollowPoint)
            {
                StopObservingBottomCard();

                CardData nextBottomCard = bottomCard.BottomCard.Value;

                bottomCard.gameObject.SetActive(false);

                if (nextBottomCard != null)
                {
                    nextBottomCard.LinkTo(_cardData);
                }

                _cardData.StoredCards.Add(bottomCard as SellableCardData);

                StopObservingBottomCardDistance();
                StartObservingBottomCard();
            }
        }
    }
}
