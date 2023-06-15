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
            _isTopCardSubscription = _cardData.IsTopCard.Subscribe(_ => OnCardsGroupUpdated());
            _isSingleCardSubscription = _cardData.IsSingleCard.Where(x => x).Subscribe(_ => OnBecameHeadOfGroup());
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
            if (_cardData.IsTopCard.Value)
            {
                OnBecameHeadOfGroup();
            }
        }

        private void OnBecameHeadOfGroup()
        {
            _cardData.Callbacks.onBecameHeadOfGroup?.Invoke();
        }
    }
}
