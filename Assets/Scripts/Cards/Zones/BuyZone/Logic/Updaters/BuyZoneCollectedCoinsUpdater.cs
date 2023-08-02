using System;
using Cards.Zones.BuyZone.Data;
using Runtime.Commands;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Zones.BuyZone.Logic.Updaters
{
    public class BuyZoneCollectedCoinsUpdater : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private BuyZoneData _buyZoneData;

        private IDisposable _collectedCoinsSubscription;

        private GameRestartCommand _gameRestartCommand;

        [Inject]
        private void Constructor(GameRestartCommand gameRestartCommand)
        {
            _gameRestartCommand = gameRestartCommand;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _buyZoneData = GetComponentInParent<BuyZoneData>(true);
        }

        private void Awake()
        {
            _gameRestartCommand.OnExecute += OnRestart;
        }

        private void OnEnable()
        {
            StartObserving();
        }

        private void OnDisable()
        {
            StopObserving();
        }

        private void OnDestroy()
        {
            _gameRestartCommand.OnExecute -= OnRestart;
        }

        #endregion

        private void StartObserving()
        {
            StopObserving();
            _collectedCoinsSubscription = _buyZoneData.CollectedCoins.Subscribe(OnCollectedCoinsUpdated);
        }

        private void StopObserving()
        {
            _collectedCoinsSubscription?.Dispose();
        }

        private void OnCollectedCoinsUpdated(int collectedCoins)
        {
            if (collectedCoins == _buyZoneData.Price.Value)
            {
                _buyZoneData.BuyZoneCallbacks.spawnCardCommand.Invoke();
                _buyZoneData.CollectedCoins.Value = 0;
                return;
            }

            if (collectedCoins > _buyZoneData.Price.Value)
            {
                _buyZoneData.CollectedCoins.Value = _buyZoneData.Price.Value;
            }
            else if (collectedCoins < 0)
            {
                _buyZoneData.CollectedCoins.Value = 0;
            }
        }

        private void OnRestart()
        {
            _buyZoneData.CollectedCoins.Value = 0;
        }
    }
}
