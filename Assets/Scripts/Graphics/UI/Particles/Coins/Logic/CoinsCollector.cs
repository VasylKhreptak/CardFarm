using Economy;
using Graphics.UI.Particles.Core;
using Graphics.UI.Particles.Logic;
using Providers.Graphics.UI;
using UnityEngine;
using Zenject;

namespace Graphics.UI.Particles.Coins.Logic
{
    public class CoinsCollector : MonoBehaviour
    {
        private CoinsBank _coinsBank;
        private ParticlesPileSpawner _particlesPileSpawner;
        private CoinIconPositionProvider _iconPositionProvider;

        [Inject]
        private void Constructor(CoinsBank coinsBank, ParticlesPileSpawner particlesPileSpawner, CoinIconPositionProvider iconPositionProvider)
        {
            _coinsBank = coinsBank;
            _particlesPileSpawner = particlesPileSpawner;
            _iconPositionProvider = iconPositionProvider;
        }

        public void Collect(int count, Vector3 position)
        {
            _particlesPileSpawner.Spawn(Particle.Coin, count, position, _iconPositionProvider.Value, OnCollectedCoin);
        }

        public void Collect(int count, Vector3 position, float maxRange)
        {
            _particlesPileSpawner.Spawn(Particle.Coin, count, position, _iconPositionProvider.Value, maxRange, OnCollectedCoin);
        }

        private void OnCollectedCoin()
        {
            _coinsBank.Add(1);
        }
    }
}
