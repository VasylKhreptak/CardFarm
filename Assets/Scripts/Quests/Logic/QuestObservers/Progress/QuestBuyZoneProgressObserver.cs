using System.Collections.Generic;
using Cards.Core;
using Cards.Data;
using Cards.Zones.BuyZone.Data;
using Quests.Logic.QuestObservers.Core;
using UniRx;
using UnityEngine;

namespace Quests.Logic.QuestObservers.Progress
{
    public class QuestBuyZoneProgressObserver : AllCardsQuestObserver
    {
        [Header("Preferences")]
        [SerializeField] private Card _targetBuyZone;

        private Dictionary<CardData, CompositeDisposable> _cardSubscriptions = new Dictionary<CardData, CompositeDisposable>();

        protected override void OnCardAdded(CardData cardData)
        {
            StartObservingCard(cardData);
        }

        protected override void OnCardRemoved(CardData cardData)
        {
            StopObservingCard(cardData);
        }

        protected override void OnCardsCleared()
        {
            StopObservingCards();
            _questData.Progress.Value = 0f;
        }

        private void StartObservingCard(CardData cardData)
        {
            if (cardData.Card.Value != _targetBuyZone) return;

            StopObservingCard(cardData);

            BuyZoneData buyZoneData = cardData as BuyZoneData;

            if (buyZoneData == null) return;

            CompositeDisposable subscriptions = new CompositeDisposable();

            buyZoneData.Price.Subscribe(_ => OnBuyZoneDataUpdated(buyZoneData)).AddTo(subscriptions);
            buyZoneData.CollectedCoins.Subscribe(_ => OnBuyZoneDataUpdated(buyZoneData)).AddTo(subscriptions);

            _cardSubscriptions.Add(cardData, subscriptions);
        }

        private void StopObservingCard(CardData cardData)
        {
            if (cardData.Card.Value != _targetBuyZone) return;

            if (_cardSubscriptions.TryGetValue(cardData, out var subscriptions))
            {
                subscriptions?.Clear();
            }

            _cardSubscriptions.Remove(cardData);
        }

        private void StopObservingCards()
        {
            foreach (var subscriptions in _cardSubscriptions.Values)
            {
                subscriptions?.Clear();
            }

            _cardSubscriptions.Clear();
        }

        private void OnBuyZoneDataUpdated(BuyZoneData buyZoneData)
        {
            int price = buyZoneData.Price.Value;
            int collectedCoins = buyZoneData.CollectedCoins.Value;

            float progress = (float)collectedCoins / price;

            SetProgress(progress);
        }

        private void SetProgress(float progress)
        {
            _questData.Progress.Value = progress;
        }
    }
}
