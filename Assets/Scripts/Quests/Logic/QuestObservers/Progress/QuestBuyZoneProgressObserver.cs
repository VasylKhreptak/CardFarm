using System.Collections.Generic;
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
            KillProgressTween();
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

            Debug.Log("Price: " + price);
            Debug.Log("CollectedCoins: " + collectedCoins);
            
            float progress = (float)collectedCoins / price;

            Debug.Log("Progress: " + progress);

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
    }
}
