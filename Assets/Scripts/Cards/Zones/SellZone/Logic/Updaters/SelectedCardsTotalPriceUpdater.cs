using System;
using System.Collections.Generic;
using Cards.Data;
using Cards.Zones.SellZone.Data;
using Table;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Zones.SellZone.Logic.Updaters
{
    public class SelectedCardsTotalPriceUpdater : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private SellZoneData _sellZoneData;

        private CardData _previousSelectedCard;

        private IDisposable _selectedCardSubscription;

        private CurrentSelectedCardHolder _selectedCardHolder;

        [Inject]
        private void Constructor(CurrentSelectedCardHolder currentSelectedCardHolder)
        {
            _selectedCardHolder = currentSelectedCardHolder;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _sellZoneData = GetComponentInParent<SellZoneData>(true);
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
            _selectedCardSubscription = _selectedCardHolder.SelectedCard.Subscribe(OnSelectedCardChanged);
        }

        private void StopObserving()
        {
            _selectedCardSubscription?.Dispose();
            StopObservingGroupCards(_previousSelectedCard);
            _sellZoneData.SelectedCardsTotalPrice.Value = 0;
            _previousSelectedCard = null;
        }

        private void OnSelectedCardChanged(CardData selectedCard)
        {
            StopObservingGroupCards(_previousSelectedCard);

            if (selectedCard == null)
            {
                _sellZoneData.SelectedCardsTotalPrice.Value = 0;
                _previousSelectedCard = null;
                return;
            }

            StartObservingGroupCards(selectedCard);
            _previousSelectedCard = selectedCard;
        }

        private void StartObservingGroupCards(CardData cardData)
        {
            OnGroupCardsUpdated(cardData.GroupCards);

            cardData.Callbacks.onGroupCardsListUpdated += OnGroupCardsUpdated;
        }

        private void StopObservingGroupCards(CardData cardData)
        {
            if (cardData == null)
            {
                return;
            }

            cardData.Callbacks.onGroupCardsListUpdated -= OnGroupCardsUpdated;
        }

        private void OnGroupCardsUpdated(List<CardData> groupCards)
        {
            int price = 0;

            foreach (var groupCard in groupCards)
            {
                if (groupCard.IsSellableCard == false) continue;

                SellableCardData sellableCardData = groupCard as SellableCardData;

                price += sellableCardData.Price.Value;
            }

            _sellZoneData.SelectedCardsTotalPrice.Value = price;
        }
    }
}
