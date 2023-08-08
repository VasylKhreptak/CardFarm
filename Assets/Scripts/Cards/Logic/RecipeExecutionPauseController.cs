using Cards.Data;
using ProgressLogic.Core;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Logic
{
    public class RecipeExecutionPauseController : MonoBehaviour, IValidatable
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
            _progressDependentObject = GetComponent<ProgressDependentObject>();
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

            _cardData.IsTopCard.Subscribe(_ => OnCardEnvironmentChanged()).AddTo(_subscriptions);
            _cardData.IsSingleCard.Subscribe(_ => OnCardEnvironmentChanged()).AddTo(_subscriptions);
            _cardData.IsAnyGroupCardSelected.Subscribe(_ => OnCardEnvironmentChanged()).AddTo(_subscriptions);
            _cardData.Callbacks.onBecameHeadOfGroup += OnCardEnvironmentChanged;
        }

        private void StopObserving()
        {
            _subscriptions.Clear();
            _cardData.Callbacks.onBecameHeadOfGroup -= OnCardEnvironmentChanged;
        }

        private void OnCardEnvironmentChanged()
        {
            bool isPaused = false;

            bool isTopCard = _cardData.IsTopCard.Value;
            bool isSingleCard = _cardData.IsSingleCard.Value;
            bool isAnyGroupCardSelected = _cardData.IsAnyGroupCardSelected.Value;

            isPaused =
                isTopCard
                && isSingleCard == false
                && isAnyGroupCardSelected;

            SetPause(isPaused);
        }

        private void SetPause(bool isPaused) => _progressDependentObject.SetPause(isPaused);
    }
}
