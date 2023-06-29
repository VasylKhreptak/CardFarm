using System;
using System.Collections.Generic;
using System.Linq;
using Cards.Boosters.Data;
using Cards.Data;
using Quests.Logic.QuestObservers.Core;
using UniRx;

namespace Quests.Logic.QuestObservers
{
    public class OpenBoosterQuest : QuestCardsObserver
    {
        private IDisposable _boostersSubscription;

        public override void StopObserving()
        {
            base.StopObserving();

            StopObservingBoosters();
        }

        protected override void OnCardsCountChanged(ReactiveCollection<CardData> cards)
        {
            StartObservingBoosters(cards);
        }

        private void StartObservingBoosters(ReactiveCollection<CardData> boosters)
        {
            StopObservingBoosters();

            List<BoosterCardData> boostersData = boosters.Select(x => x as BoosterCardData).ToList();
            List<IObservable<int>> boosterLeftCardsObservables = boostersData.Select(x => x.LeftCards as IObservable<int>).ToList();

            _boostersSubscription = Observable
                .Merge(boosterLeftCardsObservables)
                .Subscribe(leftCards =>
                {
                    if (leftCards == 0)
                    {
                        MarkQuestAsCompleted();
                        StopObserving();
                    }
                });
        }

        private void StopObservingBoosters()
        {
            _boostersSubscription?.Dispose();
        }
    }
}
