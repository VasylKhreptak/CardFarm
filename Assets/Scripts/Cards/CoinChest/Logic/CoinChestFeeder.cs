using System.Collections.Generic;
using Cards.CoinChest.Data;
using Cards.Core;
using Cards.Data;
using UnityEngine;

namespace Cards.CoinChest.Logic
{
    public class CoinChestFeeder : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CoinChestCardData _cardData;

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
                if (card.Card.Value == Card.Coin)
                {
                    _cardData.Coins.Value++;
                    card.gameObject.SetActive(false);
                }
            }
        }
    }
}
