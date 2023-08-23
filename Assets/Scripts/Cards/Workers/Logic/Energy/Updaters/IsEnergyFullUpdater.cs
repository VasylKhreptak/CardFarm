using Cards.Workers.Data;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Workers.Logic.Energy.Updaters
{
    public class IsEnergyFullUpdater : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private WorkerData _cardData;

        private CompositeDisposable _subscriptions = new CompositeDisposable();

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _cardData = GetComponentInParent<WorkerData>(true);
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

            _cardData.Energy.Subscribe(_ => UpdateValue()).AddTo(_subscriptions);
            _cardData.MaxEnergy.Subscribe(_ => UpdateValue()).AddTo(_subscriptions);
        }

        private void StopObserving()
        {
            _subscriptions.Clear();
        }

        private void UpdateValue()
        {
            int energy = _cardData.Energy.Value;
            int maxEnergy = _cardData.MaxEnergy.Value;

            _cardData.IsEnergyFull.Value = energy >= maxEnergy;
        }
    }
}
