using System.Collections;
using Cards.Workers.Data;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Workers.Logic.Energy
{
    public class EnergyRestorer : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private WorkerData _workerData;

        private CompositeDisposable _subscriptions = new CompositeDisposable();

        private Coroutine _restoringEnergyCoroutine;

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

            _workerData.EnergyRestoreDuration.Subscribe(_ => OnDataUpdated()).AddTo(_subscriptions);
            _workerData.IsEnergyFull.Subscribe(_ => OnDataUpdated()).AddTo(_subscriptions);
        }

        private void StopObserving()
        {
            _subscriptions.Clear();
        }

        private void OnDataUpdated()
        {
            bool isEnergyFull = _workerData.IsEnergyFull.Value;

            StopRestoringEnergy();

            if (isEnergyFull) return;

            StartRestoringEnergy();
        }

        private void StartRestoringEnergy()
        {
            if (_restoringEnergyCoroutine == null)
            {
                _restoringEnergyCoroutine = StartCoroutine(RestoreEnergyRoutine());
            }
        }

        private void StopRestoringEnergy()
        {
            if (_restoringEnergyCoroutine != null)
            {
                StopCoroutine(_restoringEnergyCoroutine);
                _restoringEnergyCoroutine = null;
            }
        }

        private IEnumerator RestoreEnergyRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(_workerData.EnergyRestoreDuration.Value);

                IncrementEnergy();
            }
        }

        private void IncrementEnergy()
        {
            int currentEnergy = _workerData.Energy.Value;

            currentEnergy++;

            currentEnergy = Mathf.Clamp(currentEnergy, _workerData.MinEnergy.Value, _workerData.MaxEnergy.Value);

            _workerData.Energy.Value = currentEnergy;
        }
    }
}
