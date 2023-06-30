using System.Collections.Generic;
using Cards.Chests.SellableChest.Data;
using Cards.Core;
using Cards.Data;
using UnityEngine;
using Zenject;

namespace Cards.Chests.SellableChest.Logic
{
    public class ChestFeeder : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private ChestSellableCardData _cardData;

        [Header("Preferences")]
        [SerializeField] private Card _chestType;

        #region MonoBehaviour

        public void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _cardData = GetComponentInParent<ChestSellableCardData>(true);
        }

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
            _cardData.StoredCards.Clear();
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
