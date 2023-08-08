using System;
using System.Collections.Generic;
using System.Linq;
using Cards.Data;
using Cards.Zones.SellZone.Data;
using CardsTable;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Zones.SellZone.Logic.Updaters
{
    public class SelectedCardsTotalPriceUpdater : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private SellZoneData _sellZoneData;

        private CardDataHolder _previousSelectedCard;

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

        private void OnSelectedCardChanged(CardDataHolder selectedCard)
        {
            StopObservingGroupCards(_previousSelectedCard);

            if (selectedCard == null)
            {
                _sellZoneData.SelectedCardsTotalPrice.Value = 0;
                _previousSelectedCard = null;
                return;
            }

            StartObservingBottomCards(selectedCard);
            _previousSelectedCard = selectedCard;
        }

        private void StartObservingBottomCards(CardDataHolder cardData)
        {
            OnBottomCardsUpdated();

            cardData.Callbacks.onBottomCardsListUpdated += OnBottomCardsUpdated;
        }

        private void StopObservingGroupCards(CardDataHolder cardData)
        {
            if (cardData == null)
            {
                return;
            }

            cardData.Callbacks.onGroupCardsListUpdated -= OnBottomCardsUpdated;
        }

        private void OnBottomCardsUpdated()
        {
            CardDataHolder selectedCard = _selectedCardHolder.SelectedCard.Value;

            if (selectedCard == null) return;

            List<CardDataHolder> targetCards = selectedCard.BottomCards.ToList();

            targetCards.Add(selectedCard);

            int price = 0;

            if (targetCards.First().IsSellableCard)
            {
                foreach (var targetCard in targetCards)
                {
                    if (targetCard.IsSellableCard == false) continue;

                    SellableCardData sellableCardData = targetCard as SellableCardData;

                    price += sellableCardData.Price.Value;
                }
            }

            _sellZoneData.SelectedCardsTotalPrice.Value = price;
        }
    }
}
