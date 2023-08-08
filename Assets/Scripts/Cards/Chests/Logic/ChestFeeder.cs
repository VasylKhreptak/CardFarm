using System;
using Cards.Chests.Data;
using Cards.Core;
using Cards.Data;
using Extensions;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Chests.Logic
{
    public class ChestFeeder : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private ChestData _cardData;

        [Header("Preferences")]
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
            _cardData = GetComponentInParent<ChestData>(true);
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

            if (CanCardBeAdded(bottomCard))
            {
                StartObservingBottomCardDistance();
            }
        }

        private void StartObservingBottomCardDistance()
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
                StopObservingBottomCardDistance();

                if (TryAddCardToChest(bottomCard) == false)
                {
                    bottomCard.UnlinkFromUpper();
                }
            }
        }

        private bool TryAddCardToChest(CardData card)
        {
            if (_cardData.StoredCards.Count >= _cardData.Capacity.Value) return false;

            if (CanCardBeAdded(card))
            {
                AddCardToChest(card);
                return true;
            }

            return false;
        }

        private void AddCardToChest(CardData card)
        {
            CardData nextBottomCard = card.BottomCard.Value;

            card.gameObject.SetActive(false);

            nextBottomCard.LinkTo(_cardData);

            _cardData.StoredCards.Add(card as SellableCardData);
        }

        private bool CanCardBeAdded(CardData cardData)
        {
            if (cardData == null) return false;

            Card? chestType = _cardData.ChestType.Value;

            if (cardData.CanBePlacedInChest == false) return false;

            if (chestType == null) return true;

            if (cardData.Card.Value == chestType.Value) return true;

            return false;
        }
    }
}
