﻿using System.Collections.Generic;
using Cards.Data;
using UnityEngine;

namespace Cards.Logic.Updaters
{
    public class BottomCardsListUpdater : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        #region MonoBehaviour

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
            List<CardData> bottomCards = _cardData.BottomCardsProvider.FindBottomCards();
            _cardData.BottomCards = bottomCards;

            _cardData.Callbacks.onBottomCardsListUpdated?.Invoke();
        }
    }
}