using System;
using UniRx;
using UnityEngine;
using Zenject;
using Zones.BuyZone.Data;

namespace Zones.BuyZone.Logic.Updaters
{
    public class BuyZoneCollectedAllCoinsEventInvoker : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private BuyZoneData _buyZoneData;

        private IDisposable _collectedCoinsSubscription;

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _buyZoneData = GetComponentInParent<BuyZoneData>(true);
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
            _collectedCoinsSubscription = _buyZoneData.CollectedCoinsCount.Subscribe(OnCollectedCoinsCountChanged);
        }

        private void StopObserving()
        {
            _collectedCoinsSubscription?.Dispose();
        }

        private void OnCollectedCoinsCountChanged(int collectedCoinsCount)
        {
            if (collectedCoinsCount == _buyZoneData.Price.Value)
            {
                _buyZoneData.Callbacks.OnCollectedAllCoins?.Invoke();
                _buyZoneData.CollectedCoinsCount.Value = 0;
            }
        }
    }
}
