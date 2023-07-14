using System;
using Cards.Data;
using Table;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.ResourceNodes.Core.Logic.Updaters
{
    public class IsResourceNodeCompatibleWithSelectedCardUpdater : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

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
            _cardData = GetComponentInParent<CardData>(true);
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

            isCompatible = selectedCard.IsWorker;

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
