using System;
using Data.Cards.Core;
using UniRx;
using UnityEngine;
using UnlockedCardPanel.VisualizableCard.Data;
using Zenject;

namespace UnlockedCardPanel.VisualizableCard.VisualElements.Core
{
    public abstract class VisualizableCardObserver : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private VisualizableCardData _cardData;

        private IDisposable _cardDataSubscription;

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public virtual void Validate()
        {
            _cardData ??= GetComponentInParent<VisualizableCardData>(true);
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

            _cardDataSubscription = _cardData.VisualizableCard.Subscribe(OnCardDataChanged);
        }

        private void StopObserving()
        {
            _cardDataSubscription?.Dispose();
        }

        protected abstract void OnCardDataChanged(CardDataHolder cardData);
    }
}
