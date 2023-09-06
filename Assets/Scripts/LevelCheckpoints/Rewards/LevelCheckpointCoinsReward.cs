using System;
using Graphics.UI.Particles.Coins.Logic;
using UniRx;
using UnityEngine;
using Zenject;

namespace LevelCheckpoints.Rewards
{
    public class LevelCheckpointCoinsReward : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform _rewardSpawnPoint;
        [SerializeField] private LevelCheckpoint _levelCheckpoint;

        [Header("Preferences")]
        [SerializeField] private int _coinsCount = 5;

        private IDisposable _subscription;

        private CoinsCollector _coinsCollector;

        [Inject]
        private void Constructor(CoinsCollector coinsCollector)
        {
            _coinsCollector = coinsCollector;
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
            StopObserving();

            _subscription = _levelCheckpoint.Reached.Where(x => x).Subscribe(_ => SpawnReward());
        }

        private void StopObserving()
        {
            _subscription?.Dispose();
        }

        private void SpawnReward()
        {
            _coinsCollector.Collect(_coinsCount, _rewardSpawnPoint.position);
        }
    }
}
