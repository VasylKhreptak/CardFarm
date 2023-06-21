using System;
using System.Collections.Generic;
using Cards.Core;
using Cards.Data;
using Cards.Logic.Spawn;
using Cards.Zones.BoosterBuyZone.Data;
using Coins;
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
        [SerializeField] private float _coinMoveDelay = 0.3f;

        private CompositeDisposable _delays = new CompositeDisposable();

        private CardsTable _cardsTable;
        private CardSpawner _cardSpawner;
        private CoinsProvider _coinsProvider;

        [Inject]
        private void Constructor(CardsTable cardsTable, CardSpawner cardSpawner, CoinsProvider coinsProvider)
        {
            _cardsTable = cardsTable;
            _cardSpawner = cardSpawner;
            _coinsProvider = coinsProvider;
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
        }

        private void StopObserving()
        {
            StopObservingClick();
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

        private void OnClicked()
        {
            int price = _data.BoosterPrice.Value;

            float delay = 0f;

            if (_coinsProvider.TryGetCoins(price, out List<CardData> coins))
            {
                for (int i = 0; i < price; i++)
                {
                    CardData coin = coins[i];
                    coin.gameObject.SetActive(false);

                    Observable.Timer(TimeSpan.FromSeconds(delay)).Subscribe(_ =>
                    {
                        coin.gameObject.SetActive(true);
                        coin.Animations.MoveAnimation.Play(_data.transform.position, _cardsMoveDuration, () =>
                        {
                            coin.gameObject.SetActive(false);
                        });
                    }).AddTo(_delays);

                    delay += _coinMoveDelay;
                }

                Observable.Timer(TimeSpan.FromSeconds(delay + _cardsMoveDuration - _coinMoveDelay))
                    .Subscribe(_ => SpawnBooster()).AddTo(_delays);
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
