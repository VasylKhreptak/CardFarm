using System;
using Cards.Data;
using UniRx;
using UnityEngine;

namespace Cards.Logic
{
    public class ChainBottomPartEventInvoker : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        private CardData _previousBottomCard;

        private IDisposable _bottomCardSubscription;

        #region MonoBehaviour

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

        private void OnBottomCardChanged(CardData bottomCard)
        {
            RemovePreviousCardSubscription();

            if (bottomCard != null)
            {
                bottomCard.Callbacks.onChainBottomPartUpdated += Invoke;
                _previousBottomCard = bottomCard;
            }

            Invoke();
        }

        private void RemovePreviousCardSubscription()
        {
            if (_previousBottomCard != null)
            {
                _previousBottomCard.Callbacks.onChainBottomPartUpdated -= Invoke;
            }
        }

        private void Invoke()
        {
            _cardData.Callbacks.onChainBottomPartUpdated?.Invoke();
            Debug.Log("Bottom Cards Changed");
        }
    }
}
