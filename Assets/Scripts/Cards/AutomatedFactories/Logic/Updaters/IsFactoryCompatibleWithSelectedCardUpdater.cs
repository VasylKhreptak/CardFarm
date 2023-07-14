using System;
using Cards.AutomatedFactories.Data;
using Cards.Data;
using Table;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.AutomatedFactories.Logic.Updaters
{
    public class IsFactoryCompatibleWithSelectedCardUpdater : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private AutomatedFactoryData _cardData;

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
            _cardData = GetComponentInParent<AutomatedFactoryData>(true);
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

            foreach (var recipe in _cardData.FactoryRecipes.Recipes)
            {
                if (recipe.Resources.Contains(selectedCard.Card.Value))
                {
                    isCompatible = true;
                    break;
                }
            }

            _cardData.IsCompatibleWithSelectedCard.Value = isCompatible;
        }
    }
}
