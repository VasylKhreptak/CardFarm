using System;
using Cards.Core;
using Cards.Data;
using Cards.Logic.Spawn;
using Cards.Zones.BuyZone.Data;
using Coins;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Zones.BuyZone.Logic
{
    public class BuyZoneCoinsCollector : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private BuyZoneData _cardData;

        [Header("Preferences")]
        [SerializeField] private float _cardsMoveDuration = 1f;
        [SerializeField] private float _coinMoveDelay = 0.2f;

        private CompositeDisposable _delays = new CompositeDisposable();

        private bool _canCollectCoins = true;

        private CardSpawner _cardSpawner;
        private CoinsProvider _coinsProvider;

        [Inject]
        private void Constructor(CardSpawner cardSpawner, CoinsProvider coinsProvider)
        {
            _cardSpawner = cardSpawner;
            _coinsProvider = coinsProvider;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _cardData = GetComponentInParent<BuyZoneData>(true);
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
            _cardData.Callbacks.onClicked += OnClicked;
        }

        private void StopObservingClick()
        {
            _cardData.Callbacks.onClicked -= OnClicked;
        }

        private void OnClicked()
        {
            CollectCoins();
        }

        private void CollectCoins()
        {
            if (_canCollectCoins == false) return;

            int price = _cardData.Price.Value;

            int coinsCount = _coinsProvider.GetCoinsCount();

            coinsCount = Mathf.Min(price, _cardData.LeftCoins.Value);

            if (coinsCount == 0) return;

            int coinsToSpawn = Mathf.Min(price, coinsCount);

            float delay = 0f;

            for (int i = 0; i < coinsToSpawn; i++)
            {
                Observable.Timer(TimeSpan.FromSeconds(delay)).Subscribe(_ =>
                {
                    if (_coinsProvider.TryGetCoin(out CardData coinCard))
                    {
                        coinCard.gameObject.SetActive(false);
                        CardData fakeCoin = _cardSpawner.Spawn(Card.FakeCoin, coinCard.transform.position);
                        fakeCoin.Animations.MoveAnimation.Play(_cardData.transform.position, _cardsMoveDuration, () =>
                        {
                            fakeCoin.gameObject.SetActive(false);
                            _cardData.CollectedCoins.Value++;
                        });
                    }
                }).AddTo(_delays);

                delay += _coinMoveDelay;
            }

            _canCollectCoins = false;

            Observable.Timer(TimeSpan.FromSeconds(delay + _cardsMoveDuration)).Subscribe(_ =>
            {
                _canCollectCoins = true;
            }).AddTo(_delays);
        }

        private void RemoveDelaySubscriptions()
        {
            _delays.Clear();
        }
    }
}
