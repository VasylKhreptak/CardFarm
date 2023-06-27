using System;
using System.Collections.Generic;
using System.Linq;
using Cards.Core;
using Cards.Data;
using Quests.Logic.QuestObservers.Core;
using Table.Core;
using UniRx;
using UnityEngine;
using Zenject;

namespace Quests.Logic.QuestObservers
{
    public class MoveCardOnAnotherCardQuest : QuestObserver
    {
        [Header("Preferences")]
        [SerializeField] private Card _topCard;
        [SerializeField] private Card _bottomCard;

        private IDisposable _newCardsAppearedSubscription;
        private IDisposable _topCardsSubscription;

        private CardsTableSelector _cardsTableSelector;

        [Inject]
        private void Constructor(CardsTableSelector cardsTableSelector)
        {
            _cardsTableSelector = cardsTableSelector;
        }

        public override void StartObserving()
        {
            StartObservingNewCardType();
        }

        public override void StopObserving()
        {
            StopObservingNewCardType();
        }

        private void StartObservingNewCardType()
        {
            _newCardsAppearedSubscription = _cardsTableSelector.SelectedCardsMap.ObserveAdd()
                .Subscribe(x =>
                {
                    if (x.Key == _topCard)
                    {
                        OnNewCardsAppeared(x.Value);
                    }
                });
        }

        private void StopObservingNewCardType()
        {
            _newCardsAppearedSubscription?.Dispose();
            StopObservingTopCards();
        }

        private void OnNewCardsAppeared(ReactiveCollection<CardData> cards)
        {
            StartObservingTopCards(cards);
        }

        private void StartObservingTopCards(IReadOnlyCollection<CardData> cards)
        {
            if (cards.Count == 0) return;

            StopObservingTopCards();

            IObservable<CardData>[] topCards = cards.Select(x => x.TopCard as IObservable<CardData>).ToArray();

            _topCardsSubscription = Observable.Merge(topCards).Where(x => x.Card.Value == _bottomCard)
                .Subscribe(_ =>
                {
                    _questData.IsCompleted.Value = true;
                    Debug.Log("Completed");
                });

        }

        private void StopObservingTopCards()
        {
            _topCardsSubscription?.Dispose();
        }
    }
}
