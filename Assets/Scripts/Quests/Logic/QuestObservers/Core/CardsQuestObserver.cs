using System;
using Cards.Core;
using Cards.Data;
using CardsTable.Core;
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

        private CardSelector _cardSelector;

        [Inject]
        private void Constructor(CardSelector cardSelector)
        {
            _cardSelector = cardSelector;
        }

        public override void StartObserving()
        {
            if (_cardSelector.SelectedCardsMap.TryGetValue(_targetCard, out var boosterCards))
            {
                StarObservingCardsCount(boosterCards);
            }

            _cardAppearedSubscription = _cardSelector.SelectedCardsMap.ObserveAdd().Subscribe(x =>
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

        private void StarObservingCardsCount(ReactiveCollection<CardDataHolder> boosters)
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

        protected abstract void OnCardsCountChanged(ReactiveCollection<CardDataHolder> cards);
    }
}
