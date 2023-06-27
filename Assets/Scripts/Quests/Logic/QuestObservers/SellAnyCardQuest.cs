using System;
using Cards.Core;
using Cards.Data;
using Cards.Zones.SellZone.Data;
using Quests.Logic.QuestObservers.Core;
using Table.Core;
using UniRx;
using Zenject;

namespace Quests.Logic.QuestObservers
{
    public class SellAnyCardQuest : QuestObserver
    {
        private IDisposable _newCardsAppearedSubscription;
        private IDisposable _topCardsCountSubscription;

        private SellZoneData _currentSellZoneData;

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
            StopObservingSellZone();
        }

        private void StartObservingNewCardType()
        {
            if (_cardsTableSelector.SelectedCardsMap.TryGetValue(Card.SellZone, out ReactiveCollection<CardData> cards))
            {
                _currentSellZoneData = cards[0] as SellZoneData;

                StartObservingSellZone();
            }

            _newCardsAppearedSubscription = _cardsTableSelector.SelectedCardsMap.ObserveAdd()
                .Subscribe(x =>
                {
                    if (x.Key == Card.SellZone)
                    {
                        _currentSellZoneData = x.Value[0] as SellZoneData;

                        StartObservingSellZone();
                    }
                });
        }

        private void StopObservingNewCardType()
        {
            _newCardsAppearedSubscription?.Dispose();
        }

        private void StartObservingSellZone()
        {
            if (_currentSellZoneData == null) return;

            StopObservingSellZone();

            _currentSellZoneData.onSoldCard += OnSoldCard;
        }

        private void StopObservingSellZone()
        {
            if (_currentSellZoneData == null) return;

            _currentSellZoneData.onSoldCard -= OnSoldCard;
        }

        private void OnSoldCard(Card card)
        {
            _questData.IsCompleted.Value = true;
            StopObserving();
        }
    }
}
