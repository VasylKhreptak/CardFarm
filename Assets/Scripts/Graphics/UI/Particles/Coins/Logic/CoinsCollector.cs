using Economy;
using Graphics.UI.Particles.Core;
using Graphics.UI.Particles.Data;
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
        private Transform _coinIconTransform;

        [Inject]
        private void Constructor(CoinsBank coinsBank,
            ParticlesPileSpawner particlesPileSpawner,
            CoinIconPositionProvider iconPositionProvider)
        {
            _coinsBank = coinsBank;
            _particlesPileSpawner = particlesPileSpawner;
            _coinIconTransform = iconPositionProvider.GetComponent<Transform>();
        }

        public void Collect(int count, Vector3 position)
        {
            _particlesPileSpawner.Spawn(Particle.Coin, count, position, OnSpawnedCoin);
        }

        public void Collect(int count, Vector3 position, float maxRange)
        {
            _particlesPileSpawner.Spawn(Particle.Coin, count, position, maxRange, OnSpawnedCoin);
        }

        private void OnSpawnedCoin(ParticleData coin)
        {
            coin.Animations.MoveSequence.Play(_coinIconTransform, () =>
            {
                OnCollectedCoin(coin);
            });
        }

        private void OnCollectedCoin(ParticleData coin)
        {
            _coinsBank.Add(1);
            coin.gameObject.SetActive(false);
        }
    }
}
