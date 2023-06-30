using System;
using Cards.Core;
using Cards.Data;
using Table;
using UniRx;
using UnityEngine;
using Zenject;
using IValidatable = EditorTools.Validators.Core.IValidatable;

namespace Cards.Zones.SellZone.Logic.Updaters
{
    public class IsSellZoneCompatibleWithSelectedCardUpdater : MonoBehaviour, IValidatable
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

        public void OnValidate()
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
            if (selectedCard == null || selectedCard.Card.Value == Card.Coin)
            {
                _cardData.IsCompatibleWithSelectedCard.Value = false;
                return;
            }

            _cardData.IsCompatibleWithSelectedCard.Value = selectedCard.IsSellableCard;
        }
    }
}
