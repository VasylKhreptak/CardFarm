using Cards.Data;
using Quests.Logic.QuestObservers.Core;
using UniRx;

namespace Quests.Logic.QuestObservers
{
    public class SellAnyCardQuest : QuestCardsObserver
    {

        public override void StopObserving()
        {
            base.StopObserving();
        }

        protected override void OnCardsCountChanged(ReactiveCollection<CardData> cards)
        {
            
        }
    }
}
