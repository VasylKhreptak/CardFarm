using Cards.Core;
using Cards.Data;
using Cards.Zones.SellZone.Data;
using Extensions;
using Quests.Logic.Tutorials.Core;
using UniRx;
using Zenject;

namespace Quests.Logic.Tutorials
{
    public class SellCardTutorial : QuestHandMoveTutorial
    {
        private CompositeDisposable _cardTableSubscriptions = new CompositeDisposable();

        private SellableCardData _cheapestCard;
        private SellZoneData _sellZone;

        private CardsTable.Core.CardsTable _cardsTable;

        [Inject]
        public void Construct(CardsTable.Core.CardsTable cardsTable)
        {
            _cardsTable = cardsTable;
        }

        public override void StartTutorial()
        {
            base.StartTutorial();

            StartObservingCardsTable();
        }

        public override void StopTutorial()
        {
            base.StopTutorial();

            StopObservingCardsTable();
            StopObservingSellZone(_sellZone);
            _cheapestCard = null;
            _sellZone = null;
        }

        private void StartObservingCardsTable()
        {
            StopObservingCardsTable();

            foreach (var card in _cardsTable.Cards)
            {
                OnAddedCard(card);
            }

            IReadOnlyReactiveCollection<CardData> tableCards = _cardsTable.Cards;
            tableCards.ObserveAdd().Subscribe(addEvent => OnAddedCard(addEvent.Value)).AddTo(_cardTableSubscriptions);
            tableCards.ObserveRemove().Subscribe(removeEvent => OnRemovedCard(removeEvent.Value)).AddTo(_cardTableSubscriptions);
            tableCards.ObserveReset().Subscribe(_ => OnClearCards()).AddTo(_cardTableSubscriptions);
        }

        private void StopObservingCardsTable()
        {
            _cardTableSubscriptions?.Clear();
        }

        private void OnAddedCard(CardData cardData)
        {
            SellZoneData sellZoneData = cardData as SellZoneData;

            if (sellZoneData != null)
            {
                OnAddedSellZone(sellZoneData);
            }
            else
            {
                SellableCardData sellableCardData = cardData as SellableCardData;

                if (sellableCardData != null)
                {
                    OnAddedSellableCard(sellableCardData);
                }
            }
        }

        private void OnRemovedCard(CardData cardData)
        {
            SellZoneData sellZoneData = cardData as SellZoneData;

            if (sellZoneData != null)
            {
                OnRemovedSellZone(sellZoneData);
            }
            else
            {
                SellableCardData sellableCardData = cardData as SellableCardData;

                if (sellableCardData != null)
                {
                    OnRemovedSellableCard(sellableCardData);
                }
            }
        }

        private void OnClearCards()
        {
            StartTutorial();
        }

        private void OnAddedSellableCard(SellableCardData cardData)
        {
            if (_cheapestCard == null || cardData.Price.Value < _cheapestCard.Price.Value)
            {
                _cheapestCard = cardData;

                OnFoundCardsUpdated();
            }
        }

        private void OnRemovedSellableCard(SellableCardData cardData)
        {
            if (_cheapestCard == cardData)
            {
                SellableCardData cheapestCard = null;

                foreach (var card in _cardsTable.Cards)
                {
                    SellableCardData sellableCardData = card as SellableCardData;

                    if (sellableCardData == null) continue;

                    if (cheapestCard == null || sellableCardData.Price.Value < cheapestCard.Price.Value)
                    {
                        cheapestCard = sellableCardData;
                    }
                }

                _cheapestCard = cheapestCard;

                OnFoundCardsUpdated();
            }
        }

        private void OnAddedSellZone(SellZoneData sellZoneData)
        {
            if (_sellZone == null)
            {
                StopObservingSellZone(_sellZone);
                StartObservingSellZone(sellZoneData);
                _sellZone = sellZoneData;

                OnFoundCardsUpdated();
            }
        }

        private void OnRemovedSellZone(SellZoneData sellZoneData)
        {
            if (_sellZone == sellZoneData)
            {
                StopObservingSellZone(_sellZone);

                _cardsTable.TryGetFirstCard(Card.SellZone, out var sellZone);

                _sellZone = sellZone as SellZoneData;

                OnFoundCardsUpdated();
            }
        }

        private void OnFoundCardsUpdated()
        {
            if (_cheapestCard == null || _sellZone == null)
            {
                StopHandTutorial();
                return;
            }

            TryUnlinkCheapestCard();

            StartHandTutorialDelayed(_cheapestCard.transform, _sellZone.transform);
        }

        private void StartObservingSellZone(SellZoneData sellZoneData)
        {
            StopObservingSellZone(sellZoneData);

            sellZoneData.onSoldCard += OnSoldCard;
        }

        private void StopObservingSellZone(SellZoneData sellZoneData)
        {
            if (sellZoneData == null) return;

            sellZoneData.onSoldCard -= OnSoldCard;
        }

        private void OnSoldCard(Card card)
        {
            StopTutorial();
            _isFinished.Value = true;
        }

        private void TryUnlinkCheapestCard()
        {
            if (_cheapestCard == null) return;

            if (_cheapestCard.IsSingleCard.Value == false)
            {
                _cheapestCard.Separate();
                _cheapestCard.Animations.JumpAnimation.PlayRandomly();
            }
        }

        protected override void OnRepeated()
        {
            //TryUnlinkCheapestCard();
        }
    }
}
