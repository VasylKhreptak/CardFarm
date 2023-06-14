using System;
using System.Collections.Generic;
using Cards.Data;
using UnityEngine;

namespace Cards.Logic
{
    public class BottomCardsUpdater : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

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


        }

        private void StopObservingBottomCard()
        {
            _bottomCardSubscription?.Dispose();
        }

        private void UpdateBottomCards()
        {

        }
    }
}
