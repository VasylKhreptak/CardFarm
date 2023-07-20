using System;
using Economy;
using Graphics.UI.Particles.Core;
using Graphics.UI.Particles.Logic;
using Providers.Graphics.UI;
using UnityEngine;
using Zenject;

namespace Graphics.UI.Particles.Coins.Logic
{
    public class CoinsSpender : MonoBehaviour
    {
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

        public void Spend(int count, Vector3 targetPosition, Action onCompleted = null)
        {
            int totalCoins = _coinsBank.Value;

            if (totalCoins == 0) return;

            count = Mathf.Min(totalCoins, count);

            _particlesPileSpawner.Spawn(Particle.Coin, count, _coinPositionProvider.Value, 0f, (coin) =>
            {
                OnSpentCoin();
                coin.Animations.MoveAnimation.Play(targetPosition, onCompleted);
            });
        }

        private void OnSpentCoin()
        {
            _coinsBank.TrySpend(1);
        }
    }
}
