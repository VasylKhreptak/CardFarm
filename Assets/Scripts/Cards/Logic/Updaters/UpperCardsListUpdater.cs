﻿using System.Collections.Generic;
using Cards.Data;
using UnityEngine;

namespace Cards.Logic.Updaters
{
    public class UpperCardsListUpdater : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        #region MonoBehaviour

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
            _cardData.Callbacks.onAnyBottomCardUpdated -= UpdateUpperCardsList;
        }

        private void UpdateUpperCardsList()
        {
            List<CardData> upperCards = _cardData.UpperCardsProvider.FindUpperCards();
            upperCards.Reverse();
            _cardData.UpperCards.Clear();

            foreach (var upperCard in upperCards)
            {
                _cardData.UpperCards.Add(upperCard);
            }

            _cardData.Callbacks.onUpperCardsListUpdated?.Invoke();
        }
    }
}
