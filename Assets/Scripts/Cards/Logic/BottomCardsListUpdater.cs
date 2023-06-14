using System.Collections.Generic;
using Cards.Data;
using UnityEngine;

namespace Cards.Logic
{
    public class BottomCardsListUpdater : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        #region MonoBehaviour

        private void OnEnable()
        {
            UpdateBottomCards();
            StartObservingBottomCard();
        }

        private void OnDisable()
        {
            StopObservingBottomCard();
        }

        #endregion

        private void StartObservingBottomCard()
        {
            StopObservingBottomCard();

            _cardData.Callbacks.onBottomCardsUpdated += UpdateBottomCards;
        }

        private void StopObservingBottomCard()
        {
            _cardData.Callbacks.onBottomCardsUpdated -= UpdateBottomCards;
        }

        private void UpdateBottomCards()
        {
            List<CardData> bottomCards = _cardData.BottomCardsProvider.FindBottomCards();
            _cardData.BottomCards.Clear();

            foreach (var bottomCard in bottomCards)
            {
                _cardData.BottomCards.Add(bottomCard);
            }

            _cardData.Callbacks.onBottomCardsListUpdated?.Invoke();
        }
    }
}
