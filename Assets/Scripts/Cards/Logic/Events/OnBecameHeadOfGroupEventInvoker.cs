using System;
using Cards.Data;
using UniRx;
using UnityEngine;

namespace Cards.Logic.Events
{
    public class OnBecameHeadOfGroupEventInvoker : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        private IDisposable _isTopCardSubscription;
        private IDisposable _isSingleCardSubscription;

        #region MonoBehaviour

        private void OnEnable()
        {
            _cardData.Callbacks.onGroupCardsListUpdated += OnCardsGroupUpdated;
        }

        private void OnDisable()
        {
            _cardData.Callbacks.onGroupCardsListUpdated -= OnCardsGroupUpdated;
            _isTopCardSubscription?.Dispose();
            _isSingleCardSubscription?.Dispose();
        }

        #endregion

        private void OnCardsGroupUpdated()
        {
            _isTopCardSubscription?.Dispose();
            _isSingleCardSubscription?.Dispose();
            _isTopCardSubscription = _cardData.IsTopCard.Where(x => x).Subscribe(_ => OnBecameHeadOfGroup());
            _isSingleCardSubscription = _cardData.IsSingleCard.Where(x => x).Subscribe(_ => OnBecameHeadOfGroup());
        }

        private void OnBecameHeadOfGroup()
        {
            _cardData.Callbacks.onBecameHeadOfGroup?.Invoke();
        }
    }
}
