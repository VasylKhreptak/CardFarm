using System;
using Cards.Data;
using UniRx;
using UnityEngine;

namespace Cards.Logic
{
    public class UpperCardsUpdatedEventInvoker : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        private CardData _previousUpperCard;

        private IDisposable _upperCardSubscription;

        #region MonoBehaviour

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

        private void OnUpperCardChanged(CardData upperCard)
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
