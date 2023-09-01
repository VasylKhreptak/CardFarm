using System;
using Cards.Core;
using Cards.Data;
using Cards.Zones.SellZone.Data;
using CardsTable.Core;
using Shop;
using UniRx;
using Zenject;

namespace Quests.Logic.QuestObservers.Core
{
    public abstract class AllCardsQuestObserver : QuestObserver
    {
        private CompositeDisposable _subscriptions = new CompositeDisposable();

        private IDisposable _sellZoneSubscription;

        private SellZoneData _foundSellZone;

        protected CardsTable.Core.CardsTable _cardsTable;
        protected CardSelector _cardSelector;
        protected ShopEvents _shopEvents;

        [Inject]
        private void Constructor(CardsTable.Core.CardsTable cardsTable,
            CardSelector cardSelector,
            ShopEvents shopEvents)
        {
            _cardsTable = cardsTable;
            _cardSelector = cardSelector;
            _shopEvents = shopEvents;
        }

        public override void StartObserving()
        {
            foreach (var tableCard in _cardsTable.Cards)
            {
                OnCardAdded(tableCard);
            }

            _cardsTable.Cards.ObserveAdd().Subscribe(x => OnCardAdded(x.Value)).AddTo(_subscriptions);
            _cardsTable.Cards.ObserveRemove().Subscribe(x => OnCardRemoved(x.Value)).AddTo(_subscriptions);
            _cardsTable.Cards.ObserveReset().Subscribe(_ => OnCardsCleared()).AddTo(_subscriptions);

            StartObservingSellZone();
            StartObservingShopEvents();
        }

        public override void StopObserving()
        {
            _subscriptions.Clear();
            OnCardsCleared();
            StopObservingSellZone();
            StopObservingShopEvents();
        }

        protected abstract void OnCardAdded(CardData cardData);

        protected abstract void OnCardRemoved(CardData cardData);

        protected abstract void OnCardsCleared();

        private void StartObservingSellZone()
        {
            StopObservingSellZone();

            foreach (var kvp in _cardSelector.SelectedCardsMap)
            {
                if (kvp.Key == Card.SellZone && kvp.Value.Count > 0)
                {
                    OnFoundSellZone(kvp.Value[0] as SellZoneData);

                    return;
                }
            }

            _sellZoneSubscription = _cardSelector.SelectedCardsMap.ObserveAdd().Subscribe(x =>
            {
                if (x.Key == Card.SellZone && x.Value.Count > 0)
                {
                    OnFoundSellZone(x.Value[0] as SellZoneData);
                }
            });
        }

        private void OnFoundSellZone(SellZoneData sellZoneData)
        {
            _foundSellZone = sellZoneData;

            if (_foundSellZone == null) return;

            _foundSellZone.onSoldCard += OnSoldCard;

            _sellZoneSubscription?.Dispose();
        }

        private void StopObservingSellZone()
        {
            _sellZoneSubscription?.Dispose();

            if (_foundSellZone != null)
            {
                _foundSellZone.onSoldCard -= OnSoldCard;
                _foundSellZone = null;
            }
        }

        private void StartObservingShopEvents()
        {
            StopObservingShopEvents();

            _shopEvents.onSpawnedCard += OnBoughtCard;
        }

        private void StopObservingShopEvents()
        {
            _shopEvents.onSpawnedCard -= OnBoughtCard;
        }

        protected virtual void OnSoldCard(Card card)
        {
        }

        protected virtual void OnBoughtCard(Card card)
        {
        }
    }
}
