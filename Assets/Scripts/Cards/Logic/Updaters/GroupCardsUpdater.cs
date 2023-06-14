using System.Collections.Generic;
using Cards.Data;
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
            List<CardData> GroupCards = new List<CardData>(_cardData.UpperCards.Count + 1 + _cardData.BottomCards.Count);

            GroupCards.AddRange(_cardData.UpperCards);
            GroupCards.Add(_cardData);
            GroupCards.AddRange(_cardData.BottomCards);

            _cardData.GroupCards = GroupCards;
            _cardData.Callbacks.onGroupCardsListUpdated?.Invoke();
        }
    }
}
