using System;
using Cards.Data;
using ScriptableObjects.Scripts.Cards;
using Table;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Logic.Updaters
{
    public class IsCompatibleWithSelectedCardUpdater : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;
        [SerializeField] private CompatibleCards _compatibleCards;

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
            _cardData ??= GetComponentInParent<CardData>();
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

            _cardData.IsCompatibleWithSelectedCard.Value = _compatibleCards.IsCompatible(selectedCard.Card.Value, _cardData.Card.Value);
        }
    }
}
