using System.Collections.Generic;
using System.Linq;
using Cards.Core;
using Cards.Data;
using Cards.Zones.SellZone.Data;
using Quests.Logic.QuestObservers.Core;
using UniRx;

namespace Quests.Logic.QuestObservers
{
    public class SellAnyCardQuest : CardsQuestObserver
    {
        private List<SellZoneData> _sellZones = new List<SellZoneData>();

        public override void StopObserving()
        {
            base.StopObserving();
            StopObservingSellZones();
        }

        protected override void OnCardsCountChanged(ReactiveCollection<CardDataHolder> cards)
        {
            StopObservingSellZones();

            _sellZones = cards.Select(x => x as SellZoneData).ToList();

            StartObservingSellZones();
        }

        private void StartObservingSellZones()
        {
            StopObservingSellZones();

            foreach (var sellZone in _sellZones)
            {
                sellZone.onSoldCard += OnSoldCard;
            }
        }

        private void StopObservingSellZones()
        {
            foreach (var sellZone in _sellZones)
            {
                sellZone.onSoldCard -= OnSoldCard;
            }
        }

        private void OnSoldCard(Card card)
        {
            MarkQuestAsCompleted();
        }
    }
}
