using System;
using Cards.Data;
using EditorTools.Validators.Core;
using UniRx;
using UnityEngine;

namespace Cards.Logic.Events
{
    public class OnBecameHeadOfGroupEventInvoker : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        private IDisposable _isTopCardSubscription;
        private IDisposable _isSingleCardSubscription;

        #region MonoBehaviour

        public void OnValidate()
        {
            _cardData = GetComponentInParent<CardData>(true);
        }
        
        private void OnEnable()
        {
            StartObservingIfSingleCard();
            StartObservingIfTopCard();
            _cardData.Callbacks.onGroupCardsListUpdated += OnCardsGroupUpdated;
        }

        private void OnDisable()
        {
            _cardData.Callbacks.onGroupCardsListUpdated -= OnCardsGroupUpdated;
            StopObservingIfTopCard();
            StopObservingIfSingleCard();
        }

        #endregion

        private void OnCardsGroupUpdated()
        {
            StartObservingIfSingleCard();
            StartObservingIfTopCard();
        }

        private void StartObservingIfSingleCard()
        {
            StopObservingIfSingleCard();
            _isSingleCardSubscription = _cardData.IsSingleCard.Where(x => x).Subscribe(_ => OnBecameHeadOfGroup());
        }

        private void StartObservingIfTopCard()
        {
            StopObservingIfTopCard();
            _isTopCardSubscription = _cardData.IsTopCard.Where(x => x).Subscribe(_ => OnBecameHeadOfGroup());
        }

        private void StopObservingIfSingleCard()
        {
            _isSingleCardSubscription?.Dispose();
        }

        private void StopObservingIfTopCard()
        {
            _isTopCardSubscription?.Dispose();
        }

        private void OnBecameHeadOfGroup()
        {
            _cardData.Callbacks.onBecameHeadOfGroup?.Invoke();
            StopObservingIfTopCard();
            StopObservingIfSingleCard();
        }
    }
}
