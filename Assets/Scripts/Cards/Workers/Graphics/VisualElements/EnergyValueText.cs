using Cards.Workers.Data;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Workers.Graphics.VisualElements
{
    public class EnergyValueText : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private WorkerData _cardData;
        [SerializeField] private TMP_Text _tmp;

        [Header("Preferences")]
        [SerializeField] private string _format = "{0}/{1}";

        private CompositeDisposable _subscriptions = new CompositeDisposable();

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _cardData = GetComponentInParent<WorkerData>(true);
            _tmp ??= GetComponent<TMP_Text>();
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

            _cardData.Energy.Subscribe(_ => UpdateText()).AddTo(_subscriptions);
            _cardData.MaxEnergy.Subscribe(_ => UpdateText()).AddTo(_subscriptions);
        }

        private void StopObserving()
        {
            _subscriptions.Clear();
        }

        private void UpdateText()
        {
            int energy = _cardData.Energy.Value;
            int maxEnergy = _cardData.MaxEnergy.Value;

            _tmp.text = string.Format(_format, energy, maxEnergy);
        }
    }
}
