using System.Collections.Generic;
using Cards.Data;
using Extensions;
using UnityEngine;
using Zenject;

namespace Cards.Logic.Updaters
{
    public class GroupCardsUpdater : MonoBehaviour, IValidatable
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
