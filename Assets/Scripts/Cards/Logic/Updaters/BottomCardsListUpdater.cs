using System.Collections.Generic;
using Cards.Data;
using EditorTools.Validators.Core;
using Extensions;
using UnityEngine;

namespace Cards.Logic.Updaters
{
    public class BottomCardsListUpdater : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        #region MonoBehaviour

        public void OnValidate()
        {
            _cardData = GetComponentInParent<CardData>(true);
        }
        
        private void OnEnable()
        {
            UpdateBottomCardsList();
            StartObservingBottomCards();
        }

        private void OnDisable()
        {
            StopObservingBottomCard();
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
            List<CardData> bottomCards = _cardData.FindBottomCards();
            _cardData.BottomCards = bottomCards;

            _cardData.Callbacks.onBottomCardsListUpdated?.Invoke();
        }
    }
}
