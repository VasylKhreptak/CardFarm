﻿using System.Collections.Generic;
using Cards.Data;
using Extensions;
using UnityEngine;

namespace Cards.Logic.Updaters
{
    public class GroupCardsUpdater : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        #region MonoBehaviour

        private void OnEnable()
        {
            OnCardsUpdated();
            _cardData.Callbacks.onUpperCardsListUpdated += OnCardsUpdated;
            _cardData.Callbacks.onBottomCardsListUpdated += OnCardsUpdated;
        }

        private void OnDisable()
        {
            _cardData.Callbacks.onUpperCardsListUpdated -= OnCardsUpdated;
            _cardData.Callbacks.onBottomCardsListUpdated -= OnCardsUpdated;
        }

        #endregion

        private void OnCardsUpdated()
        {
            List<CardData> GroupCards = _cardData.FindGroupCards();

            _cardData.GroupCards = GroupCards;
            _cardData.Callbacks.onGroupCardsListUpdated?.Invoke();
        }
    }
}
