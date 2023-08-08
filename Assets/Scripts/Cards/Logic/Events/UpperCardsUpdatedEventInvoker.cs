using System;
using Cards.Data;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Logic.Events
{
    public class UpperCardsUpdatedEventInvoker : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardDataHolder _cardData;

        private CardDataHolder _previousUpperCard;

        private IDisposable _upperCardSubscription;

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
            StartObservingUpperCard();
        }

        private void OnDisable()
        {
            StopObservingUpperCard();
        }

        #endregion

        private void StartObservingUpperCard()
        {
            StopObservingUpperCard();
            _upperCardSubscription = _cardData.UpperCard.Subscribe(OnUpperCardChanged);
        }

        private void StopObservingUpperCard()
        {
            _upperCardSubscription?.Dispose();

            RemovePreviousCardSubscription();
        }

        private void OnUpperCardChanged(CardDataHolder upperCard)
        {
            RemovePreviousCardSubscription();

            if (upperCard != null)
            {
                upperCard.Callbacks.onAnyUpperCardUpdated += Invoke;
                _previousUpperCard = upperCard;
            }

            Invoke();
        }

        private void RemovePreviousCardSubscription()
        {
            if (_previousUpperCard != null)
            {
                _previousUpperCard.Callbacks.onAnyUpperCardUpdated -= Invoke;
            }
        }

        private void Invoke()
        {
            _cardData.Callbacks.onAnyUpperCardUpdated?.Invoke();
        }
    }
}
