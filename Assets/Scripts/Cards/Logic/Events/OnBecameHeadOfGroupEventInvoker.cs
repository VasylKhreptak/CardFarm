using Cards.Data;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Logic.Events
{
    public class OnBecameHeadOfGroupEventInvoker : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

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
            StopObserving();

            _cardData.IsTopCard.Subscribe(_ => OnCardEnvironmentChanged()).AddTo(_subscriptions);
            _cardData.IsSingleCard.Subscribe(_ => OnCardEnvironmentChanged()).AddTo(_subscriptions);
            _cardData.Callbacks.onGroupCardsListUpdated += OnCardEnvironmentChanged;
        }

        private void StopObserving()
        {
            _subscriptions.Clear();
            _cardData.Callbacks.onGroupCardsListUpdated -= OnCardEnvironmentChanged;
        }

        private void OnCardEnvironmentChanged()
        {
            bool isTopCard = _cardData.IsTopCard.Value;
            bool isSingleCard = _cardData.IsSingleCard.Value;

            if (isTopCard || isSingleCard)
            {
                _cardData.Callbacks.onBecameHeadOfGroup?.Invoke();
            }
        }
    }
}
