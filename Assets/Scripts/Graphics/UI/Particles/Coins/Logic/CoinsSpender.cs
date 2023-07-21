using System;
using Economy;
using Graphics.UI.Particles.Core;
using Graphics.UI.Particles.Logic;
using Providers.Graphics.UI;
using UniRx;
using UnityEngine;
using Zenject;

namespace Graphics.UI.Particles.Coins.Logic
{
    public class CoinsSpender : MonoBehaviour
    {
        private CompositeDisposable _delays = new CompositeDisposable();

        private CoinIconPositionProvider _coinPositionProvider;
        private ParticlesPileSpawner _particlesPileSpawner;
        private CoinsBank _coinsBank;

        [Inject]
        private void Constructor(CoinIconPositionProvider iconPositionProvider,
            ParticlesPileSpawner particlesPileSpawner,
            CoinsBank coinsBank)
        {
            _coinPositionProvider = iconPositionProvider;
            _particlesPileSpawner = particlesPileSpawner;
            _coinsBank = coinsBank;
        }

        #region MonoBehaviour

        private void OnDisable()
        {
            _delays.Clear();
        }

        #endregion

        public void Spend(int count, Func<Vector3> targetPositionGetter, Action onSpentCoin = null, Action onSpentAllCoins = null)
        {
            int totalCoins = _coinsBank.Value;

            if (totalCoins == 0) return;

            count = Mathf.Min(totalCoins, count);

            _coinsBank.TrySpend(count);

            float coinMoveDuration = 0;

            _particlesPileSpawner.Spawn(Particle.Coin, count, _coinPositionProvider.Value, 0f,
                coin =>
                {
                    coin.Animations.MoveAnimation.Play(targetPositionGetter, () =>
                    {
                        onSpentCoin?.Invoke();
                        coin.gameObject.SetActive(false);
                    });
                    coinMoveDuration = coin.Animations.MoveAnimation.Duration;
                }, () =>
                {
                    Observable.Timer(TimeSpan.FromSeconds(coinMoveDuration)).Subscribe(_ =>
                    {
                        onSpentAllCoins?.Invoke();
                    }).AddTo(_delays);
                });
        }
    }
}
