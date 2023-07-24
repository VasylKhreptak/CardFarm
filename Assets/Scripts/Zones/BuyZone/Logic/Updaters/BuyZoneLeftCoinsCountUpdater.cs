using UniRx;
using UnityEngine;
using Zenject;
using Zones.BuyZone.Data;

namespace Zones.BuyZone.Logic.Updaters
{
    public class BuyZoneLeftCoinsCountUpdater : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private BuyZoneData _buyZoneData;

        private CompositeDisposable _subscriptions = new CompositeDisposable();

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
            _buyZoneData.CollectedCoinsCount.Subscribe(_ => UpdateLeftCoinsCount()).AddTo(_subscriptions);
            _buyZoneData.Price.Subscribe(_ => UpdateLeftCoinsCount()).AddTo(_subscriptions);
        }

        private void StopObserving()
        {
            _subscriptions.Clear();
        }

        private void UpdateLeftCoinsCount()
        {
            _buyZoneData.LeftCoinsCount.Value = _buyZoneData.Price.Value - _buyZoneData.CollectedCoinsCount.Value;
        }
    }
}
