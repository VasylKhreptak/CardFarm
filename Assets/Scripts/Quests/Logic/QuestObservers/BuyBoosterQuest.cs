using System;
using Cards.Core;
using Cards.Data;
using Cards.Zones.BoosterBuyZone.Data;
using Quests.Logic.QuestObservers.Core;
using Table.Core;
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

        private BoosterBuyZoneData _currentBoosterBuyZone;

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
            StopObservingButZone();
        }

        private void StartObservingNewCardType()
        {
            if (_cardsTableSelector.SelectedCardsMap.TryGetValue(_targetBuyZone, out ReactiveCollection<CardData> cards))
            {
                _currentBoosterBuyZone = GetTargetBuyZone(cards);

                StartObservingBuyZone();
            }

            _newCardsAppearedSubscription = _cardsTableSelector.SelectedCardsMap.ObserveAdd()
                .Subscribe(x =>
                {
                    if (x.Key == Card.SellZone)
                    {
                        _currentBoosterBuyZone = GetTargetBuyZone(x.Value);

                        StartObservingBuyZone();
                    }
                });
        }

        private BoosterBuyZoneData GetTargetBuyZone(ReactiveCollection<CardData> cards)
        {
            foreach (var buyZone in cards)
            {
                BoosterBuyZoneData buyZoneData = cards[0] as BoosterBuyZoneData;

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

            StopObservingButZone();

            _currentBoosterBuyZone.onSpawnedBooster += OnSpawnedBooster;
        }

        private void StopObservingButZone()
        {
            if (_currentBoosterBuyZone == null) return;

            _currentBoosterBuyZone.onSpawnedBooster -= OnSpawnedBooster;
        }

        private void OnSpawnedBooster()
        {
            _questData.IsCompleted.Value = true;
            StopObserving();
        }
    }
}
