using System.Collections.Generic;
using Cards.Data;
using Extensions;
using UnityEngine;
using Zenject;

namespace Cards.Logic.Updaters
{
    public class BottomCardsListUpdater : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardDataHolder _cardData;

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _cardData = GetComponentInParent<CardDataHolder>(true);
        }

        private void OnEnable()
        {
            UpdateBottomCardsList();
            StartObservingBottomCards();
        }

        private void OnDisable()
        {
            StopObservingBottomCard();
            _cardData.BottomCards.Clear();
            _cardData.Callbacks.onBottomCardsListUpdated?.Invoke();
        }

        #endregion

        private void StartObservingBottomCards()
        {
            StopObservingBottomCard();

            _cardData.Callbacks.onAnyBottomCardUpdated += UpdateBottomCardsList;
        }

        private void StopObservingBottomCard()
        {
            _cardData.Callbacks.onAnyBottomCardUpdated -= UpdateBottomCardsList;
        }

        private void UpdateBottomCardsList()
        {
            List<CardDataHolder> bottomCards = _cardData.FindBottomCards();
            _cardData.BottomCards = bottomCards;

            _cardData.Callbacks.onBottomCardsListUpdated?.Invoke();
        }
    }
}
