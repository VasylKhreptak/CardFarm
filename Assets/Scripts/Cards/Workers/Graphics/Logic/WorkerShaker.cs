using System;
using Cards.Core;
using Cards.Data;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Workers.Graphics.Logic
{
    public class WorkerShaker : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardDataHolder _cardData;

        private IDisposable _firstGroupCardSubscription;

        private CardDataHolder _previousFirstGroupCard;

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _cardData = GetComponentInParent<CardDataHolder>(true);
        }

        private void OnEnable()
        {
            StartObservingFirstGroupCard();
        }

        private void OnDisable()
        {
            StopObservingFirstGroupCard();
            StopObservingRecipeExecution();
        }

        #endregion

        private void StartObservingFirstGroupCard()
        {
            _firstGroupCardSubscription = _cardData.FirstGroupCard.Subscribe(OnFirstGroupCardChanged);
        }

        private void StopObservingFirstGroupCard()
        {
            _firstGroupCardSubscription?.Dispose();
        }

        private void OnFirstGroupCardChanged(CardDataHolder cardData)
        {
            if (cardData == null)
            {
                StopObservingRecipeExecution();
                return;
            }

            StartObservingRecipeExecution(cardData);

            _previousFirstGroupCard = cardData;
        }

        private void StartObservingRecipeExecution(CardDataHolder cardData)
        {
            StopObservingRecipeExecution();

            cardData.Callbacks.onSpawnedRecipeResult += OnSpawnedRecipeResult;
        }

        private void StopObservingRecipeExecution()
        {
            if (_previousFirstGroupCard == null) return;

            _previousFirstGroupCard.Callbacks.onSpawnedRecipeResult -= OnSpawnedRecipeResult;
        }

        private void OnSpawnedRecipeResult(Card card)
        {
            Shake();
        }

        private void Shake()
        {
            _cardData.Animations.ShakeAnimation.Play();
        }
    }
}
