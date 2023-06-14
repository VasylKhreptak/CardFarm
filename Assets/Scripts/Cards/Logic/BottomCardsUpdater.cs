using System;
using System.Collections.Generic;
using System.Linq;
using Cards.Data;
using UniRx;
using UnityEngine;

namespace Cards.Logic
{
    public class BottomCardsUpdater : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;


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
        }

        private void UpdateBottomCards()
        {
            List<CardData> bottomCards = _cardData.BottomCardsProvider.FindBottomCards();
            Debug.Log("BottomCardsUpdater.UpdateBottomCards()");
            _cardData.BottomCards.Clear();

            foreach (var bottomCard in bottomCards)
            {
                _cardData.BottomCards.Add(bottomCard);
            }

            _cardData.BottomCardsList = bottomCards.ToList();
        }
    }
}
