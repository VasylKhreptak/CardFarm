using System.Collections.Generic;
using Cards.Chests.Core.Data;
using Cards.Core;
using Cards.Data;
using UnityEngine;

namespace Cards.Chests.Core.Logic
{
    public class ChestFeeder : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private ChestCardData _cardData;

        [Header("Preferences")]
        [SerializeField] private Card _chestType;

        #region MonoBehaviour

        private void OnEnable()
        {
            StartObserving();
        }

        private void OnDisable()
        {
            StopObserving();
        }

        #endregion

        private void StartObserving()
        {
            _cardData.Callbacks.onBottomCardsListUpdated += OnBottomCardsUpdated;
        }

        private void StopObserving()
        {
            _cardData.Callbacks.onBottomCardsListUpdated -= OnBottomCardsUpdated;
        }

        private void OnBottomCardsUpdated()
        {
            List<CardData> bottomCards = _cardData.BottomCards;

            foreach (var card in bottomCards)
            {
                if (card.Card.Value == _chestType && card.IsSellableCard)
                {
                    _cardData.StoredCards.Add(card as SellableCardData);
                    card.gameObject.SetActive(false);
                }
            }
        }
    }
}
