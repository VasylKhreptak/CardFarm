using System;
using UniRx;
using UnityEngine;
using Zenject;
using Zones.BuyZone.Data;

namespace Zones.BuyZone.Logic.Updaters
{
    public class CollectedCoinsCountClamper : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private BuyZoneData _buyZoneData;

        private IDisposable _subscription;

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
            _subscription = _buyZoneData.CollectedCoinsCount.Subscribe(ClampCollectedCoinsCount);
        }

        private void StopObserving()
        {
            _subscription.Dispose();
        }

        private void ClampCollectedCoinsCount(int collectedCoinsCount)
        {
            if (collectedCoinsCount > _buyZoneData.Price.Value)
            {
                _buyZoneData.CollectedCoinsCount.Value = _buyZoneData.Price.Value;
            }
            else if (collectedCoinsCount < 0)
            {
                _buyZoneData.CollectedCoinsCount.Value = 0;
            }
        }
    }
}
