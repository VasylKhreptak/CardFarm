using System;
using System.Collections.Generic;
using System.Linq;
using Cards.Core;
using Cards.Data;
using Cards.Logic.Spawn;
using Cards.Zones.SellZone.Data;
using Extensions;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Zones.SellZone.Logic
{
    public class SellZoneLogic : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private SellZoneData _zoneData;

        private CompositeDisposable _delaySubscriptions = new CompositeDisposable();

        private CardSpawner _cardSpawner;

        [Inject]
        private void Constructor(CardSpawner cardSpawner)
        {
            _cardSpawner = cardSpawner;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _zoneData = GetComponentInParent<SellZoneData>(true);
        }

        private void OnEnable()
        {
            StartObserving();
        }

        private void OnDisable()
        {
            StopObserving();

            _delaySubscriptions.Clear();
        }

        #endregion

        private void StartObserving()
        {
            _zoneData.Callbacks.onBottomCardsListUpdated += OnBottomCardsUpdated;
        }

        private void StopObserving()
        {
            _zoneData.Callbacks.onBottomCardsListUpdated -= OnBottomCardsUpdated;
        }

        private void OnBottomCardsUpdated()
        {
            TrySellBottomCards();
        }

        private void TrySellBottomCards()
        {
            List<CardData> bottomCards = _zoneData.BottomCards.ToList();

            if (bottomCards.Count == 0) return;

            if (bottomCards.Count >= 1)
            {
                bottomCards[0].UnlinkFromUpper();
            }

            float delay = 0f;

            foreach (var bottomCard in bottomCards)
            {
                if (bottomCard.IsSellableCard == false) return;

                if (bottomCard.Card.Value == Card.Coin) return;

                SellableCardData sellableCard = bottomCard as SellableCardData;

                if (sellableCard == null) return;

                for (int i = 0; i < sellableCard.Price.Value; i++)
                {
                    Observable.Timer(TimeSpan.FromSeconds(_zoneData.CoinSpawnInterval)).Subscribe(_ =>
                    {
                        _cardSpawner.SpawnAndMove(Card.Coin, _zoneData.transform.position, _zoneData.CoinSpawnPoint.position, flip: false, jump: false);
                    }).AddTo(_delaySubscriptions);
                }

                sellableCard.gameObject.SetActive(false);
                _zoneData.onSoldCard?.Invoke(sellableCard.Card.Value);

                delay += _zoneData.CoinSpawnInterval;
            }
        }
    }
}
