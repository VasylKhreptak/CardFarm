using Cards.Data;
using Graphics.Shaders;
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

        private CircularProgress _circularProgress;

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
            _cardData.GearsDrawer.GearsObject.Subscribe(_ => OnCardDataUpdated()).AddTo(_subscriptions);
            _progressDependentObject.Progress.Subscribe(_ => OnCardDataUpdated()).AddTo(_subscriptions);
        }

        private void StopObserving()
        {
            _subscriptions.Clear();
        }

        private void OnCardDataUpdated()
        {
            GameObject gears = _cardData.GearsDrawer.GearsObject.Value;

            if (gears == null)
            {
                _circularProgress = null;
                return;
            }

            _circularProgress ??= gears.GetComponentInChildren<CircularProgress>();

            float progress = _progressDependentObject.Progress.Value;

            _circularProgress.SetProgress(progress);
        }
    }
}
