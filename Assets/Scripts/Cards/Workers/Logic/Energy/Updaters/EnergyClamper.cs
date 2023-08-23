using Cards.Workers.Data;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Workers.Logic.Energy.Updaters
{
    public class EnergyClamper : MonoBehaviour, IValidatable
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

            _cardData.Energy.Subscribe(_ => UpdateEnergy()).AddTo(_subscriptions);
            _cardData.MinEnergy.Subscribe(_ => UpdateEnergy()).AddTo(_subscriptions);
            _cardData.MaxEnergy.Subscribe(_ => UpdateEnergy()).AddTo(_subscriptions);
        }

        private void StopObserving()
        {
            _subscriptions.Clear();
        }

        private void UpdateEnergy()
        {
            int energy = _cardData.Energy.Value;

            int minEnergy = _cardData.MinEnergy.Value;
            int maxEnergy = _cardData.MaxEnergy.Value;

            if (energy < minEnergy)
            {
                energy = minEnergy;
            }
            else if (energy > maxEnergy)
            {
                energy = maxEnergy;
            }

            _cardData.Energy.Value = energy;
        }
    }
}
