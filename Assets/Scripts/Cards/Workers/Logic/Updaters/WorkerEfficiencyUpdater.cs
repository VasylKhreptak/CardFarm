using Cards.Workers.Data;
using Extensions;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Workers.Logic.Updaters
{
    public class WorkerEfficiencyUpdater : MonoBehaviour, IValidatable
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

        #endregion

        private void StartObserving()
        {
            StopObserving();

            _workerData.Satiety.Subscribe(_ => UpdateEfficiency()).AddTo(_subscriptions);
            _workerData.MinSatiety.Subscribe(_ => UpdateEfficiency()).AddTo(_subscriptions);
            _workerData.MaxSatiety.Subscribe(_ => UpdateEfficiency()).AddTo(_subscriptions);
            _workerData.MinEfficiency.Subscribe(_ => UpdateEfficiency()).AddTo(_subscriptions);
            _workerData.MaxEfficiency.Subscribe(_ => UpdateEfficiency()).AddTo(_subscriptions);
        }

        private void StopObserving()
        {
            _subscriptions.Clear();
        }

        private void UpdateEfficiency()
        {
            int satiety = _workerData.Satiety.Value;
            int minSatiety = _workerData.MinSatiety.Value;
            int maxSatiety = _workerData.MaxSatiety.Value;
            float efficiency;
            float minEfficiency = _workerData.MinEfficiency.Value;
            float maxEfficiency = _workerData.MaxEfficiency.Value;
            AnimationCurve curve = _workerData.SatietyToEfficiencyCurve;

            efficiency = curve.Evaluate(minSatiety, maxSatiety, satiety, minEfficiency, maxEfficiency);

            _workerData.Efficiency.Value = efficiency;
        }
    }
}
