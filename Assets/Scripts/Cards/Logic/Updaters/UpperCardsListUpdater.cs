using System.Collections.Generic;
using Cards.Data;
using EditorTools.Validators.Core;
using Extensions;
using UnityEngine;

namespace Cards.Logic.Updaters
{
    public class UpperCardsListUpdater : MonoBehaviour, IValidatable
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
            UpdateUpperCardsList();
            StartObservingUpperCards();
        }

        private void OnDisable()
        {
            StopObservingUpperCards();
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
