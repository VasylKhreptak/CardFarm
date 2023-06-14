using System;
using Cards.Data;
using UniRx;
using UnityEngine;

namespace Cards.Logic.BoolStateUpdaters
{
    public class IsMiddleCardUpdater : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        private IDisposable _subscription;

        #region MonoBehaviour

        private void OnEnable()
        {
            StartObserving();
        }

        private void OnDisable()
        {
            StopObserving();
        }

        #endregion

        private void StartObserving()
        {
            StopObserving();

            IObservable<CardData> upperCardObservable = _cardData.UpperCard;
            IObservable<CardData> bottomCardObservable = _cardData.BottomCard;

            _subscription = Observable.CombineLatest(upperCardObservable, bottomCardObservable)
                .Subscribe(cards => OnCardsUpdated(cards[0], cards[1]));
        }

        private void StopObserving()
        {
            _subscription?.Dispose();
        }

        private void OnCardsUpdated(CardData upperCard, CardData bottomCard)
        {
            _cardData.IsMiddleCard.Value = upperCard != null && bottomCard != null;
        }
    }
}
