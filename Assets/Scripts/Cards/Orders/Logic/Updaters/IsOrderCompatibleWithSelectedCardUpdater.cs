using System;
using Cards.Data;
using Cards.Orders.Data;
using Table;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Orders.Logic.Updaters
{
    public class IsOrderCompatibleWithSelectedCardUpdater : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private OrderData _cardData;

        private IDisposable _selectedCardSubscription;

        private CurrentSelectedCardHolder _selectedCardHolder;

        [Inject]
        private void Constructor(CurrentSelectedCardHolder selectedCardHolder)
        {
            _selectedCardHolder = selectedCardHolder;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _cardData = GetComponentInParent<OrderData>(true);
        }

        private void OnEnable()
        {
            StartObservingSelectedCard();
        }

        private void OnDisable()
        {
            StopObservingSelectedCard();
        }

        #endregion

        private void StartObservingSelectedCard()
        {
            StopObservingSelectedCard();
            _selectedCardSubscription = _selectedCardHolder.SelectedCard.Subscribe(OnSelectedCardUpdated);
        }

        private void StopObservingSelectedCard()
        {
            _selectedCardSubscription?.Dispose();
        }

        private void OnSelectedCardUpdated(CardData selectedCard)
        {
            if (selectedCard == null || selectedCard == _cardData)
            {
                _cardData.IsCompatibleWithSelectedCard.Value = false;
                return;
            }

            bool isCompatible = false;

            isCompatible = selectedCard.Card.Value == _cardData.OrderRequiredCard;

            if (isCompatible == false)
            {
                if (selectedCard.Card.Value == _cardData.Card.Value && selectedCard.IsStackable && _cardData.CanBeUnderCards)
                {
                    isCompatible = true;
                }
            }

            _cardData.IsCompatibleWithSelectedCard.Value = isCompatible;
        }
    }
}
