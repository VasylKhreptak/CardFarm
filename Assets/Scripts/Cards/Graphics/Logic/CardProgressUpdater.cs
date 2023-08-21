using Cards.Data;
using Graphics.Shaders;
using Graphics.VisualElements.Gears;
using ProgressLogic.Core;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Graphics.Logic
{
    public class CardProgressUpdater : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;
        [SerializeField] private ProgressDependentObject _progressDependentObject;

        private CompositeDisposable _subscriptions = new CompositeDisposable();

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _cardData = GetComponentInParent<CardData>(true);
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
            _cardData.GearsDrawer.Gears.Subscribe(_ => OnCardDataUpdated()).AddTo(_subscriptions);
            _progressDependentObject.Progress.Subscribe(_ => OnCardDataUpdated()).AddTo(_subscriptions);
        }

        private void StopObserving()
        {
            _subscriptions.Clear();
        }

        private void OnCardDataUpdated()
        {
            GearsData gears = _cardData.GearsDrawer.Gears.Value;

            if (gears == null) return;

            float progress = _progressDependentObject.Progress.Value;

            gears.CircularProgress.SetProgress(progress);
        }
    }
}
