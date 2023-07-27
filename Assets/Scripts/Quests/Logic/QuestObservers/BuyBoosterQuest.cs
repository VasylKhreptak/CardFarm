using System;
using Cards.Core;
using Cards.Data;
using Cards.Zones.BuyZone.Data;
using CardsTable.Core;
using Quests.Logic.QuestObservers.Core;
using UniRx;
using UnityEngine;
using Zenject;

namespace Quests.Logic.QuestObservers
{
    public class BuyBoosterQuest : QuestObserver
    {
        [Header("Preferences")]
        [SerializeField] private Card _targetBuyZone;

        private IDisposable _newCardsAppearedSubscription;
        private IDisposable _zonesCountSubscription;

        private BuyZoneData _currentBoosterBuyZone;

        private CardSelector _cardSelector;

        [Inject]
        private void Constructor(CardSelector cardSelector)
        {
            _cardSelector = cardSelector;
        }

        public override void StartObserving()
        {
            StartObservingNewCardType();
        }

        public override void StopObserving()
        {
            StopObservingNewCardType();
            StopObservingBuyZone();
            StopObservingZonesCount();
        }

        private void StartObservingNewCardType()
        {
            if (_cardSelector.SelectedCardsMap.TryGetValue(_targetBuyZone, out ReactiveCollection<CardData> cards))
            {
                StartObservingZonesCount(cards);
            }

            _newCardsAppearedSubscription = _cardSelector.SelectedCardsMap.ObserveAdd()
                .Subscribe(x =>
                {
                    if (x.Key == _targetBuyZone)
                    {
                        StartObservingZonesCount(x.Value);
                    }
                });
        }

        private void StartObservingZonesCount(ReactiveCollection<CardData> cards)
        {
            StopObservingZonesCount();
            _zonesCountSubscription = cards.ObserveCountChanged()
                .DoOnSubscribe(() => OnZonesCountChanged(cards.Count, cards))
                .Subscribe(x =>
                {
                    OnZonesCountChanged(x, cards);
                });
        }

        private void OnZonesCountChanged(int count, ReactiveCollection<CardData> cards)
        {
            if (count > 0)
            {
                _currentBoosterBuyZone = GetTargetBuyZone(cards);
                StopObservingZonesCount();
                StartObservingBuyZone();
            }
            else
            {
                StopObservingBuyZone();
            }
        }

        private void StopObservingZonesCount()
        {
            _zonesCountSubscription?.Dispose();
        }

        private BuyZoneData GetTargetBuyZone(ReactiveCollection<CardData> cards)
        {
            foreach (var buyZone in cards)
            {
                BuyZoneData buyZoneData = buyZone as BuyZoneData;

                if (buyZoneData == null) continue;

                if (buyZoneData.Card.Value == _targetBuyZone)
                {
                    return buyZoneData;
                }
            }

            return null;
        }

        private void StopObservingNewCardType()
        {
            _newCardsAppearedSubscription?.Dispose();
        }

        private void StartObservingBuyZone()
        {
            if (_currentBoosterBuyZone == null) return;

            StopObservingBuyZone();

            _currentBoosterBuyZone.BuyZoneCallbacks.onSpawnedCard += OnSpawnedBooster;
        }

        private void StopObservingBuyZone()
        {
            if (_currentBoosterBuyZone == null) return;

            _currentBoosterBuyZone.BuyZoneCallbacks.onSpawnedCard -= OnSpawnedBooster;
        }

        private void OnSpawnedBooster()
        {
            _questData.IsCompleted.Value = true;
            StopObserving();
        }
    }
}
