using System;
using Cards.Data;
using Cards.Factories.Data;
using Cards.Workers.Data;
using ScriptableObjects.Scripts.Cards.AutomatedFactories.Recipes;
using ScriptableObjects.Scripts.Cards.Recipes;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Workers.Logic.Energy
{
    public class WorkerEnergyRecipeConsumer : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private WorkerData _cardData;

        private IDisposable _topCardSubscription;

        private CardData _previousFirstGroupCard;

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _cardData = GetComponentInParent<WorkerData>(true);
        }

        private void OnEnable()
        {
            StartObservingFirstGroupCard();
        }

        private void OnDisable()
        {
            StopObservingFirstGroupCard();
            StopObservingRecipeExecution(_previousFirstGroupCard);
        }

        #endregion

        private void StartObservingFirstGroupCard()
        {
            StopObservingFirstGroupCard();

            _topCardSubscription = _cardData.FirstGroupCard.Subscribe(OnFirstGroupCardUpdated);
        }

        private void StopObservingFirstGroupCard()
        {
            _topCardSubscription?.Dispose();
        }

        private void OnFirstGroupCardUpdated(CardData cardData)
        {
            StartObservingRecipeExecution(cardData);
        }

        private void StartObservingRecipeExecution(CardData cardData)
        {
            StopObservingRecipeExecution(_previousFirstGroupCard);
            StopObservingRecipeExecution(cardData);

            if (cardData == null) return;

            if (cardData is FactoryData factoryData)
            {
                factoryData.AutomatedFactoryCallbacks.onExecutedRecipe += OnExecutedRecipe;
            }
            else
            {
                cardData.Callbacks.onExecutedRecipe += OnExecutedRecipe;
            }

            _previousFirstGroupCard = cardData;
        }

        private void StopObservingRecipeExecution(CardData cardData)
        {
            if (cardData == null) return;

            cardData.Callbacks.onExecutedRecipe -= OnExecutedRecipe;

            if (cardData is FactoryData factoryData)
            {
                factoryData.AutomatedFactoryCallbacks.onExecutedRecipe -= OnExecutedRecipe;
            }
        }

        private void OnExecutedRecipe(CardRecipe cardRecipe)
        {
            ConsumeEnergy(cardRecipe.EnergyCost);
        }

        private void OnExecutedRecipe(FactoryRecipe factoryRecipe)
        {
            ConsumeEnergy(factoryRecipe.EnergyCost);
        }

        private void ConsumeEnergy(int energy)
        {
            int currentEnergy = _cardData.Energy.Value;

            _cardData.Energy.Value = Mathf.Clamp(currentEnergy - energy, _cardData.MinEnergy.Value, _cardData.MaxEnergy.Value);
        }
    }
}
