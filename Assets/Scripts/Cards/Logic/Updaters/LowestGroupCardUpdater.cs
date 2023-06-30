﻿using System.Linq;
using Cards.Data;
using UnityEngine;
using Zenject;

namespace Cards.Logic.Updaters
{
    public class LowestGroupCardUpdater : MonoBehaviour, IValidatable
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
            OnGroupCardsUpdated();
            StartObservingGroupCards();
        }

        private void OnDisable()
        {
            StopObservingGroupCards();
        }

        #endregion

        private void StartObservingGroupCards()
        {
            StopObservingGroupCards();

            _cardData.Callbacks.onGroupCardsListUpdated += OnGroupCardsUpdated;
        }

        private void StopObservingGroupCards()
        {
            _cardData.Callbacks.onGroupCardsListUpdated -= OnGroupCardsUpdated;
        }

        private void OnGroupCardsUpdated()
        {
            if (_cardData.GroupCards.Count == 0)
            {
                _cardData.LowestGroupCard.Value = null;
                return;
            }

            _cardData.LowestGroupCard.Value = _cardData.GroupCards.Last();
        }
    }
}
