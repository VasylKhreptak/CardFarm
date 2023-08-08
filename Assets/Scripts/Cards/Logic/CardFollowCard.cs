using System;
using Cards.Data;
using Constraints.CardTable;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Logic
{
    public class CardFollowCard : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardDataHolder _cardData;

        [Header("Preferences")]
        [SerializeField] private float _speed = 5f;

        private IDisposable _topCardSubscription;
        private IDisposable _updateDisposable;

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
            _cardData = GetComponentInParent<CardDataHolder>(true);
        }

        private void OnEnable()
        {
            StartObservingTopCard();
        }

        private void OnDisable()
        {
            StopObservingTopCard();
            StopFollowing();
        }

        #endregion

        private void StartObservingTopCard()
        {
            StopObservingTopCard();
            _topCardSubscription = _cardData.UpperCard.Subscribe(topCard =>
            {
                if (topCard == null)
                {
                    StopFollowing();
                }
                else
                {
                    StartFollowing();
                }
            });
        }

        private void StopObservingTopCard()
        {
            _topCardSubscription?.Dispose();
        }

        private void StartFollowing()
        {
            StopFollowing();
            _updateDisposable = Observable.EveryUpdate().Subscribe(_ => FollowStep());
        }

        private void StopFollowing()
        {
            _updateDisposable?.Dispose();
        }

        private void FollowStep()
        {
            if (_cardData.IsSelected.Value) return;

            Vector3 targetPosition = _cardData.UpperCard.Value.BottomCardFollowPoint.position;
            Vector3 transformPosition = _cardData.transform.position;

            transformPosition = Vector3.Lerp(transformPosition, targetPosition, _speed * Time.deltaTime);
            transformPosition.y = _cardData.Height.Value;

            Vector3 clampedPosition = _cardsTableBounds.Clamp(_cardData.RectTransform, transformPosition);
            _cardData.transform.position = clampedPosition;
        }
    }
}
