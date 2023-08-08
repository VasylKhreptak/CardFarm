using System;
using Cards.Data;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Logic.Updaters
{
    public class IsMiddleCardUpdater : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardDataHolder _cardData;

        private IDisposable _subscription;

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
            StartObserving();
        }

        private void OnDisable()
        {
            StopObserving();
            _cardData.IsMiddleCard.Value = false;
        }

        #endregion

        private void StartObserving()
        {
            StopObserving();

            IObservable<CardDataHolder> upperCardObservable = _cardData.UpperCard;
            IObservable<CardDataHolder> bottomCardObservable = _cardData.BottomCard;

            _subscription = Observable.CombineLatest(upperCardObservable, bottomCardObservable)
                .Subscribe(cards => OnCardsUpdated(cards[0], cards[1]));
        }

        private void StopObserving()
        {
            _subscription?.Dispose();
        }

        private void OnCardsUpdated(CardDataHolder upperCard, CardDataHolder bottomCard)
        {
            _cardData.IsMiddleCard.Value = upperCard != null && bottomCard != null;
        }
    }
}
