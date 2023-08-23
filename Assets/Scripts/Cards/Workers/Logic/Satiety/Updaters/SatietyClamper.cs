using Cards.Workers.Data;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Workers.Logic
{
    public class SatietyClamper : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private WorkerData _workerData;

        private CompositeDisposable _subscriptions = new CompositeDisposable();

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _workerData = GetComponentInParent<WorkerData>(true);
        }

        private void OnEnable()
        {
            StartObserving();
        }

        private void OnDisable()
        {
            StopObserving();
        }

        private void StartObserving()
        {
            StopObserving();

            _workerData.Satiety.Subscribe(_ => UpdateSatiety()).AddTo(_subscriptions);
            _workerData.MinSatiety.Subscribe(_ => UpdateSatiety()).AddTo(_subscriptions);
            _workerData.MaxSatiety.Subscribe(_ => UpdateSatiety()).AddTo(_subscriptions);
        }

        private void StopObserving()
        {
            _subscriptions.Clear();
        }

        #endregion

        private void UpdateSatiety()
        {
            int satiety = _workerData.Satiety.Value;

            if (satiety < _workerData.MinSatiety.Value)
            {
                satiety = _workerData.MinSatiety.Value;
            }
            else if (satiety > _workerData.MaxSatiety.Value)
            {
                satiety = _workerData.MaxSatiety.Value;
            }

            _workerData.Satiety.Value = satiety;
        }
    }
}
