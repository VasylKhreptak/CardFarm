using System;
using Cards.Core;
using Cards.Data;
using Cards.Logic.Spawn;
using Cards.Zones.BoosterBuyZone.Data;
using Table.Core;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Zones.BoosterBuyZone.Logic
{
    public class BoosterBuyZoneLogic : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private BoosterBuyZoneData _data;

        [Header("Preferences")]
        [SerializeField] private Card _booster;
        [SerializeField] private float _cardsMoveDuration = 1f;

        private CardData[] _coinsBuffer;

        private IDisposable _priceSubscription;
        private CompositeDisposable _delays = new CompositeDisposable();

        private CardsTable _cardsTable;
        private CardSpawner _cardSpawner;

        [Inject]
        private void Constructor(CardsTable cardsTable, CardSpawner cardSpawner)
        {
            _cardsTable = cardsTable;
            _cardSpawner = cardSpawner;
        }

        #region MonoBehaviour

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
            StartObservingClick();
            StartObservingPrice();
        }

        private void StopObserving()
        {
            StopObservingClick();
            StopObservingPrice();
            RemoveDelaySubscriptions();
        }

        private void StartObservingClick()
        {
            StopObservingClick();
            _data.Callbacks.onClicked += OnClicked;
        }

        private void StopObservingClick()
        {
            _data.Callbacks.onClicked -= OnClicked;
        }

        private void StartObservingPrice()
        {
            StopObservingPrice();
            _priceSubscription = _data.BoosterPrice.Subscribe(OnPriceUpdated);
        }

        private void StopObservingPrice()
        {
            _priceSubscription?.Dispose();
        }

        private void OnPriceUpdated(int price)
        {
            _coinsBuffer = new CardData[price];
        }

        private void OnClicked()
        {
            int foundCoins = _cardsTable.TryGetLowestGroupCards(Card.Coin, ref _coinsBuffer);

            if (foundCoins == _data.BoosterPrice.Value)
            {
                for (int i = 0; i < foundCoins; i++)
                {
                    CardData coin = _coinsBuffer[i];

                    if (coin == null) continue;

                    coin.Animations.MoveAnimation.Play(transform.position, _cardsMoveDuration, () =>
                    {
                        coin.gameObject.SetActive(false);
                    });
                }

                Observable.Timer(TimeSpan.FromSeconds(_cardsMoveDuration)).Subscribe(_ => SpawnBooster()).AddTo(_delays);
            }
        }

        private void SpawnBooster()
        {
            _cardSpawner.Spawn(_booster, _data.BoosterSpawnPoint.position);
        }

        private void RemoveDelaySubscriptions()
        {
            _delays.Clear();
        }
    }
}
