using System;
using Cards.Core;
using Cards.Data;
using Cards.Logic.Spawn;
using Cards.Zones.BoosterBuyZone.Data;
using Coins;
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

        private bool _canSpawn = true;

        private CompositeDisposable _delays = new CompositeDisposable();

        private CardSpawner _cardSpawner;
        private CoinsProvider _coinsProvider;

        [Inject]
        private void Constructor(CardSpawner cardSpawner, CoinsProvider coinsProvider)
        {
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
            TrySpawnBooster();
        }

        private void TrySpawnBooster()
        {
            if (_canSpawn == false) return;

            int price = _data.BoosterPrice.Value;

            if (_coinsProvider.GetCoinsCount() < price) return;

            float delay = 0f;

            for (int i = 0; i < price; i++)
            {
                Observable.Timer(TimeSpan.FromSeconds(delay)).Subscribe(_ =>
                {
                    if (_coinsProvider.TryGetCoin(out CardData coinCard))
                    {
                        coinCard.gameObject.SetActive(false);
                        CardData fakeCoin = _cardSpawner.Spawn(Card.FakeCoin, coinCard.transform.position);
                        fakeCoin.Animations.MoveAnimation.Play(_data.transform.position, _cardsMoveDuration, () =>
                        {
                            fakeCoin.gameObject.SetActive(false);
                        });
                    }
                }).AddTo(_delays);

                delay += _coinMoveDelay;
            }

            _canSpawn = false;
            Observable.Timer(TimeSpan.FromSeconds(delay + _cardsMoveDuration - _coinMoveDelay))
                .Subscribe(_ =>
                {
                    SpawnBooster();
                    _canSpawn = true;
                }).AddTo(_delays);
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
