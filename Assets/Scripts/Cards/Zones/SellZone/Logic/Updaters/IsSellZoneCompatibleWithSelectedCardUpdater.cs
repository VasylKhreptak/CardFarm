using System;
using Cards.Data;
using Table;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Zones.SellZone.Logic.Updaters
{
    public class IsSellZoneCompatibleWithSelectedCardUpdater : MonoBehaviour
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
            if (selectedCard == null)
            {
                _cardData.IsCompatibleWithSelectedCard.Value = false;
                return;
            }

            _cardData.IsCompatibleWithSelectedCard.Value = selectedCard.IsSellableCard;
        }
    }
}
