﻿using System.Linq;
using Cards.Data;
using UnityEngine;
using Zenject;

namespace Cards.Logic.Updaters
{
    public class FirstGroupCardUpdater : MonoBehaviour, IValidatable
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
            OnGroupCardsUpdated();
            StartObservingGroupCards();
        }

        private void OnDisable()
        {
            StopObservingGroupCards();
            _cardData.FirstGroupCard.Value = null;
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
                _cardData.FirstGroupCard.Value = null;
                return;
            }

            _cardData.FirstGroupCard.Value = _cardData.GroupCards.First();
        }
    }
}
