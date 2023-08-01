﻿using System;
using Cards.Data;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Logic.Updaters
{
    public class IsBottomCardUpdater : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        private IDisposable _subscription;

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _cardData = GetComponentInParent<CardData>(true);
        }

        private void OnEnable()
        {
            StartObserving();
        }

        private void OnDisable()
        {
            StopObserving();
            _cardData.IsBottomCard.Value = false;
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
            _cardData.IsBottomCard.Value = upperCard != null && bottomCard == null;
        }
    }
}
