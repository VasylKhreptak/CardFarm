using System;
using System.Collections.Generic;
using System.Linq;
using Cards.Data;
using Tools.Bounds;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Logic.Updaters
{
    public class OverlappingCardsUpdater : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardDataHolder _cardData;

        [Header("Preferences")]
        [SerializeField] private float _updateInterval = 0.5f;
        [SerializeField] private float _maxDistance = 10f;

        private IDisposable _updateIntervalSubscription;
        private CompositeDisposable _subscriptions = new CompositeDisposable();

        private CardsTable.Core.CardsTable _cardsTable;

        [Inject]
        private void Constructor(CardsTable.Core.CardsTable cardsTable)
        {
            _cardsTable = cardsTable;
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
            StartObserving();
        }

        private void OnDisable()
        {
            StopObserving();
            _cardData.OverlappingCards.Clear();
        }

        #endregion

        private void StartObserving()
        {
            _cardData.IsAnyGroupCardSelected.Subscribe(_ => OnEnvironmentChanged()).AddTo(_subscriptions);
        }

        private void StopObserving()
        {
            StopUpdating();
            _subscriptions.Clear();
        }

        private void OnEnvironmentChanged()
        {
            if (_cardData.IsAnyGroupCardSelected.Value == false)
            {
                StartUpdating();
            }
            else
            {
                StopUpdating();
            }
        }

        private void StartUpdating()
        {
            StopUpdating();
            _updateIntervalSubscription = Observable
                .Interval(TimeSpan.FromSeconds(_updateInterval))
                .DoOnSubscribe(UpdateOverlappingCards)
                .Subscribe(_ => UpdateOverlappingCards());
        }

        private void StopUpdating()
        {
            _updateIntervalSubscription?.Dispose();
            _cardData.OverlappingCards.Clear();
        }

        private void UpdateOverlappingCards()
        {
            List<CardDataHolder> overlappingCards = new List<CardDataHolder>();

            List<CardDataHolder> cardsToCheck = _cardsTable.Cards.ToList();

            cardsToCheck.RemoveAll(card =>
            {
                bool canRemove =
                    card == _cardData
                    || card.IsSelected.Value
                    || card.GroupID.Value == _cardData.GroupID.Value
                    || card.IsZone
                    || card.IsAnyGroupCardSelected.Value
                    || card.IsPlayingAnyAnimation.Value
                    || Vector3.Distance(card.transform.position, _cardData.transform.position) > _maxDistance;

                return canRemove;
            });

            for (int i = 0; i < cardsToCheck.Count; i++)
            {
                CardDataHolder cardToCheck = cardsToCheck[i];

                if (_cardData.RectTransform.IsOverlapping(cardToCheck.RectTransform))
                {
                    overlappingCards.Add(cardToCheck);
                }
            }

            _cardData.OverlappingCards = overlappingCards;
        }

        private void OnDrawGizmosSelected()
        {
            if (_cardData == null) return;

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_cardData.transform.position, _maxDistance);
        }
    }
}
