using System;
using Cards.Data;
using Cards.Factories.Data;
using CardsTable;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Factories.Logic.Updaters
{
    public class IsFactoryCompatibleWithSelectedCardUpdater : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private FactoryData _cardData;

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
            _cardData = GetComponentInParent<FactoryData>(true);
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

        private void OnSelectedCardUpdated(CardDataHolder selectedCard)
        {
            if (selectedCard == null || selectedCard == _cardData)
            {
                _cardData.IsCompatibleWithSelectedCard.Value = false;
                return;
            }

            bool isCompatible = false;

            foreach (var recipe in _cardData.FactoryRecipes.Recipes)
            {
                if (recipe.Resources.Contains(selectedCard.Card.Value))
                {
                    isCompatible = true;
                    break;
                }
            }

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
