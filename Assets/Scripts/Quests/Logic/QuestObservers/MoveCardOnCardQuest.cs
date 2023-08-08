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

        protected override void OnCardsCountChanged(ReactiveCollection<CardDataHolder> cards)
        {
            StartObservingTopCards(cards);
        }

        private void StartObservingTopCards(ReactiveCollection<CardDataHolder> cards)
        {
            StopObservingTopCards();

            List<IObservable<CardDataHolder>> topCardsObservables = cards.Select(x => x.FirstUpperCard as IObservable<CardDataHolder>).ToList();

            _topCardsSubscription = topCardsObservables
                .Merge()
                .Where(x => x != null && x.Card.Value == _targetTopCard)
                .Subscribe(_ => MarkQuestAsCompleted());
        }

        private void StopObservingTopCards()
        {
            _topCardsSubscription?.Dispose();
        }
    }
}
