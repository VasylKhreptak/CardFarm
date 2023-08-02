using Cards.Zones.BuyZone.Data;

namespace Quests.Logic.Tutorials
{
    public class QuestBuyZoneTutorial : QuestCardClickTutorial
    {
        private BuyZoneData _buyZoneData;

        protected override void OnCardClicked()
        {
            BuyZoneData buyZoneData = _foundCard as BuyZoneData;

            if (_buyZoneData == null) return;

            StartObservingBuyZone(buyZoneData);

            _buyZoneData = buyZoneData;
        }

        public override void StopTutorial()
        {
            base.StopTutorial();

            StopObservingBuyZone();
        }

        private void StartObservingBuyZone(BuyZoneData buyZoneData)
        {
            StopObservingBuyZone();

            buyZoneData.BuyZoneCallbacks.onSpawnedCard += OnSpawnedCard;
        }

        private void StopObservingBuyZone()
        {
            if (_buyZoneData == null) return;

            _buyZoneData.BuyZoneCallbacks.onSpawnedCard -= OnSpawnedCard;
        }

        private void OnSpawnedCard()
        {
            StopTutorial();
        }
    }
}
