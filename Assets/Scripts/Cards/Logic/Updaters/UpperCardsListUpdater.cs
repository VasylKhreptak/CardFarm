using System.Collections.Generic;
using Cards.Data;
using Extensions;
using UnityEngine;
using Zenject;

namespace Cards.Logic.Updaters
{
    public class UpperCardsListUpdater : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

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
            UpdateUpperCardsList();
            StartObservingUpperCards();
        }

        private void OnDisable()
        {
            StopObservingUpperCards();
            _cardData.UpperCards.Clear();
            _cardData.Callbacks.onUpperCardsListUpdated?.Invoke();
        }

        #endregion

        private void StartObservingUpperCards()
        {
            StopObservingUpperCards();

            _cardData.Callbacks.onAnyUpperCardUpdated += UpdateUpperCardsList;
        }

        private void StopObservingUpperCards()
        {
            _cardData.Callbacks.onAnyUpperCardUpdated -= UpdateUpperCardsList;
        }

        private void UpdateUpperCardsList()
        {
            List<CardData> upperCards = _cardData.FindUpperCards();
            _cardData.UpperCards = upperCards;

            _cardData.Callbacks.onUpperCardsListUpdated?.Invoke();
        }
    }
}
