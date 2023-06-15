using Cards.Data;
using UnityEngine;

namespace Cards.Logic.Events
{
    public class OnBecameHeadOfGroupEventInvoker : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        #region MonoBehaviour

        private void OnEnable()
        {
            _cardData.Callbacks.onGroupCardsListUpdated += OnCardsGroupUpdated;
        }

        private void OnDisable()
        {
            _cardData.Callbacks.onGroupCardsListUpdated -= OnCardsGroupUpdated;
        }

        #endregion

        private void OnCardsGroupUpdated()
        {
            if (_cardData.IsTopCard.Value)
            {
                _cardData.Callbacks.onBecameHeadOfGroup?.Invoke();
                Debug.Log("Became A Head");
            }
        }
    }
}
