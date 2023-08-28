using System;
using System.Collections.Generic;
using System.Linq;
using Cards.Core;
using Cards.Data;
using Quests.Logic.QuestObservers.Core;
using UniRx;
using UnityEngine;

namespace Quests.Logic.QuestObservers
{
    public class MoveCardOnCardQuest : CardsQuestObserver
    {
        [Header("Preferences")]
        [SerializeField] private Card _targetTopCard;

        private IDisposable _topCardsSubscription;

        public override void StopObserving()
        {
            base.StopObserving();

            StopObservingTopCards();
        }

        protected override void OnCardsCountChanged(ReactiveCollection<CardData> cards)
        {
            StartObservingTopCards(cards);
        }

        private void StartObservingTopCards(ReactiveCollection<CardData> cards)
        {
            StopObservingTopCards();

            List<IObservable<CardData>> topCardsObservables = cards.Select(x => x.FirstUpperCard as IObservable<CardData>).ToList();

            _topCardsSubscription = topCardsObservables
                .Merge()
                .Where(x => x != null && x.Card.Value == _targetTopCard)
                .Subscribe(_ => MarkQuestAsCompletedByAction());
        }

        private void StopObservingTopCards()
        {
            _topCardsSubscription?.Dispose();
        }
    }
}
