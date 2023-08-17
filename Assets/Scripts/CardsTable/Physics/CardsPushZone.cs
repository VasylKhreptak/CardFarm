using System;
using System.Collections.Generic;
using System.Linq;
using Cards.Data;
using Constraints.CardTable;
using Tools.Bounds;
using UniRx;
using UnityEngine;
using Zenject;

namespace CardsTable.Physics
{
    public class CardsPushZone : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RectTransform _rectTransform;

        [Header("Preferences")]
        [SerializeField] private float _overlappingCardsUpdateInterval = 0.3f;

        [Header("Push Preferences")]
        [SerializeField] private Vector3 _direction;
        [SerializeField] private float _speed = 1f;

        private IDisposable _intervalSubscription;

        public List<CardData> _overlappingCards = new List<CardData>();

        private Core.CardsTable _cardsTable;
        private CardsTableBounds _cardsTableBounds;

        [Inject]
        private void Constructor(Core.CardsTable cardsTable, CardsTableBounds cardsTableBounds)
        {
            _cardsTable = cardsTable;
            _cardsTableBounds = cardsTableBounds;
        }
        
        #region MonoBehaviour

        private void OnValidate()
        {
            _rectTransform ??= GetComponent<RectTransform>();
            _direction = _direction.normalized;
        }

        private void OnEnable()
        {
            StartUpdatingOverlappingCards();
        }

        private void Update()
        {
            PushOverlappingCards();
        }

        private void OnDisable()
        {
            StopUpdatingOverlappingCards();
        }

        #endregion

        private void StartUpdatingOverlappingCards()
        {
            StopUpdatingOverlappingCards();
            _intervalSubscription = Observable
                .Interval(TimeSpan.FromSeconds(_overlappingCardsUpdateInterval))
                .DoOnSubscribe(UpdateOverlappingCards)
                .Subscribe(_ => UpdateOverlappingCards());
        }

        private void StopUpdatingOverlappingCards()
        {
            _intervalSubscription?.Dispose();
        }

        private void UpdateOverlappingCards()
        {
            List<CardData> cardsToCheck = _cardsTable.Cards.ToList();

            cardsToCheck.RemoveAll(card =>
            {
                bool canRemove =
                    card.IsSelected.Value
                    // || card.Animations.JumpAnimation.IsPlaying.Value
                    // || card.Animations.AppearAnimation.IsPlaying.Value
                    || card.IsAnyGroupCardSelected.Value
                    // || card.IsPlayingAnyAnimation.Value
                    || card.FirstGroupCard.Value != card
                    || card.IsZone
                    || card.RectTransform.IsOverlapping(_rectTransform) == false;

                return canRemove;
            });

            _overlappingCards = cardsToCheck;
        }

        private void PushOverlappingCards()
        {
            foreach (var overlappedCard in _overlappingCards)
            {
                Vector3 cardPosition = overlappedCard.transform.position;

                cardPosition += _direction * (_speed * Time.deltaTime);

                cardPosition = _cardsTableBounds.Clamp(overlappedCard.RectTransform, cardPosition);

                overlappedCard.transform.position = cardPosition;
            }
        }
    }
}
