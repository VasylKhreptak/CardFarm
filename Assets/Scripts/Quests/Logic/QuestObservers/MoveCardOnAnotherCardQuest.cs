using System;
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
        private IDisposable _topCardsCountSubscription;
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
            StopObservingTopCardsCount();
            StopObservingTopCards();
        }

        private void StartObservingNewCardType()
        {
            _newCardsAppearedSubscription = _cardsTableSelector.SelectedCardsMap.ObserveAdd()
                .Subscribe(x =>
                {
                    if (x.Key == _topCard)
                    {
                        StartObservingTopCardsCount(x.Value);
                    }
                });
        }

        private void StopObservingNewCardType()
        {
            _newCardsAppearedSubscription?.Dispose();
        }

        private void StartObservingTopCardsCount(ReactiveCollection<CardData> cards)
        {
            StopObservingTopCardsCount();

            _topCardsCountSubscription = cards.ObserveCountChanged().Subscribe(count =>
            {
                StartObservingTopCards(cards);
            });
        }

        private void StopObservingTopCardsCount()
        {
            _topCardsCountSubscription?.Dispose();
        }

        private void StartObservingTopCards(ReactiveCollection<CardData> cards)
        {
            StopObservingTopCards();

            IObservable<CardData>[] topCardObservables = cards.Select(x => x.TopCard as IObservable<CardData>).ToArray();

            Debug.Log(topCardObservables.Length);

            _topCardsSubscription = Observable.Merge(topCardObservables)
                .Where(x => x != null && x.Card.Value == _bottomCard)
                .Subscribe(_ =>
                {
                    _questData.IsCompleted.Value = true;
                    StopObserving();
                });
        }

        private void StopObservingTopCards()
        {
            _topCardsSubscription?.Dispose();
        }
    }
}
