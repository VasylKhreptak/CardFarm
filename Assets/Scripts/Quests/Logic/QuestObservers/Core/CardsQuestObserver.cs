using System;
using Cards.Core;
using Cards.Data;
using Table.Core;
using UniRx;
using UnityEngine;
using Zenject;

namespace Quests.Logic.QuestObservers.Core
{
    public abstract class CardsQuestObserver : QuestObserver
    {
        [Header("Preferences")]
        [SerializeField] private Card _targetCard;

        private IDisposable _cardAppearedSubscription;
        private IDisposable _cardsCountSubscription;

        private CardsTableSelector _cardsTableSelector;

        [Inject]
        private void Constructor(CardsTableSelector cardsTableSelector)
        {
            _cardsTableSelector = cardsTableSelector;
        }

        public override void StartObserving()
        {
            if (_cardsTableSelector.SelectedCardsMap.TryGetValue(_targetCard, out var boosterCards))
            {
                StarObservingCardsCount(boosterCards);
            }

            _cardAppearedSubscription = _cardsTableSelector.SelectedCardsMap.ObserveAdd().Subscribe(x =>
            {
                if (x.Key == _targetCard)
                {
                    StarObservingCardsCount(x.Value);
                }
            });
        }

        public override void StopObserving()
        {
            _cardAppearedSubscription?.Dispose();
            StopObservingCardsCount();
        }

        private void StarObservingCardsCount(ReactiveCollection<CardData> boosters)
        {
            StopObservingCardsCount();

            _cardsCountSubscription = boosters
                .ObserveCountChanged()
                .DoOnSubscribe(() => OnCardsCountChanged(boosters))
                .Subscribe(_ => OnCardsCountChanged(boosters));
        }

        private void StopObservingCardsCount()
        {
            _cardsCountSubscription?.Dispose();
        }

        protected abstract void OnCardsCountChanged(ReactiveCollection<CardData> cards);
    }
}
