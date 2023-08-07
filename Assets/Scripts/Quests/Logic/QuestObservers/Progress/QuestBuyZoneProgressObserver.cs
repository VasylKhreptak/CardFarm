using System.Collections.Generic;
using System.Linq;
using Cards.Core;
using Cards.Data;
using Cards.Zones.BuyZone.Data;
using DG.Tweening;
using Quests.Logic.QuestObservers.Core;
using UniRx;
using UnityEngine;

namespace Quests.Logic.QuestObservers.Progress
{
    public class QuestBuyZoneProgressObserver : AllCardsQuestObserver
    {
        [Header("Preferences")]
        [SerializeField] private Card _targetBuyZone;

        [Header("Preferences")]
        [SerializeField] private float _progressUpdateDuration = 0.2f;

        private Tween _progressTween;

        private Dictionary<BuyZoneData, CompositeDisposable> _cardSubscriptions = new Dictionary<BuyZoneData, CompositeDisposable>();

        protected override void OnCardAdded(CardData cardData)
        {
            StartObservingCard(cardData);
        }

        protected override void OnCardRemoved(CardData cardData)
        {
            BuyZoneData buyZoneData = cardData as BuyZoneData;

            if (buyZoneData == null) return;

            StopObservingCard(buyZoneData);
        }

        protected override void OnCardsCleared()
        {
            StopObservingCards();
            _questData.Progress.Value = 0f;
            KillProgressTween();
        }

        private void StartObservingCard(CardData cardData)
        {
            if (cardData.Card.Value != _targetBuyZone) return;

            BuyZoneData buyZoneData = cardData as BuyZoneData;

            if (buyZoneData == null) return;

            StopObservingCard(buyZoneData);

            CompositeDisposable subscriptions = new CompositeDisposable();

            buyZoneData.Price.Subscribe(_ => OnBuyZoneDataUpdated(buyZoneData)).AddTo(subscriptions);
            buyZoneData.CollectedCoins.Subscribe(_ => OnBuyZoneDataUpdated(buyZoneData)).AddTo(subscriptions);

            StartObservingBoughtCard(buyZoneData);

            _cardSubscriptions.Add(buyZoneData, subscriptions);
        }

        private void StopObservingCard(BuyZoneData cardData)
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
            foreach (var keyValuePair in _cardSubscriptions.ToList())
            {
                keyValuePair.Value?.Clear();
                StopObservingCard(keyValuePair.Key);
            }

            _cardSubscriptions.Clear();
        }

        private void OnBuyZoneDataUpdated(BuyZoneData buyZoneData)
        {
            int price = buyZoneData.Price.Value;
            int collectedCoins = buyZoneData.CollectedCoins.Value;

            float progress = (float)collectedCoins / price;

            SetProgressSmooth(progress);
        }

        private void SetProgress(float progress)
        {
            _questData.Progress.Value = progress;
        }

        private void SetProgressSmooth(float progress)
        {
            KillProgressTween();

            _progressTween = DOTween
                .To(() => _questData.Progress.Value, x => _questData.Progress.Value = x, progress, _progressUpdateDuration)
                .SetEase(Ease.OutCubic)
                .Play();
        }

        private void KillProgressTween()
        {
            _progressTween?.Kill();
        }

        private void StartObservingBoughtCard(BuyZoneData cardData)
        {
            StopObservingBoughtCard(cardData);

            cardData.BuyZoneCallbacks.onSpawnedCard += OnBoughtCard;
        }

        private void StopObservingBoughtCard(BuyZoneData cardData)
        {
            cardData.BuyZoneCallbacks.onSpawnedCard -= OnBoughtCard;
        }

        private void OnBoughtCard()
        {
            StopObservingCards();
            SetProgressSmooth(1f);
        }
    }
}
