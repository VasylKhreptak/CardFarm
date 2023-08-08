using System;
using Cards.Data;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Logic.Events
{
    public class BottomCardsUpdatedEventInvoker : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardDataHolder _cardData;

        private CardDataHolder _previousBottomCard;

        private IDisposable _bottomCardSubscription;

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
            StartObservingBottomCard();
        }

        private void OnDisable()
        {
            StopObservingBottomCard();
        }

        #endregion

        private void StartObservingBottomCard()
        {
            StopObservingBottomCard();
            _bottomCardSubscription = _cardData.BottomCard.Subscribe(OnBottomCardChanged);
        }

        private void StopObservingBottomCard()
        {
            _bottomCardSubscription?.Dispose();

            RemovePreviousCardSubscription();
        }

        private void OnBottomCardChanged(CardDataHolder bottomCard)
        {
            RemovePreviousCardSubscription();

            if (bottomCard != null)
            {
                bottomCard.Callbacks.onAnyBottomCardUpdated += Invoke;
                _previousBottomCard = bottomCard;
            }

            Invoke();
        }

        private void RemovePreviousCardSubscription()
        {
            if (_previousBottomCard != null)
            {
                _previousBottomCard.Callbacks.onAnyBottomCardUpdated -= Invoke;
            }
        }

        private void Invoke()
        {
            _cardData.Callbacks.onAnyBottomCardUpdated?.Invoke();
        }
    }
}
