using Cards.Workers.Data;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Workers.Logic.Satiety.Updaters
{
    public class NeededSatietyUpdater : MonoBehaviour, IValidatable
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
            _cardData.Satiety.Subscribe(x => UpdateNeededSatiety()).AddTo(_subscriptions);
            _cardData.MaxSatiety.Subscribe(x => UpdateNeededSatiety()).AddTo(_subscriptions);
        }

        private void StopObserving()
        {
            _subscriptions.Clear();
        }

        private void UpdateNeededSatiety()
        {
            int satiety = _cardData.Satiety.Value;
            int maxSatiety = _cardData.MaxSatiety.Value;

            _cardData.NeededSatiety.Value = maxSatiety - satiety;
        }
    }
}
