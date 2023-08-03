using System;
using System.Collections.Generic;
using System.Linq;
using Cards.Core;
using Cards.Data;
using UniRx;
using UnityEngine;
using Zenject;

namespace Quests.Logic.Tutorials.Core
{
    public class QuestMoveCardOnCardTutorial : QuestHandMoveTutorial
    {
        [Header("Preferences")]
        [SerializeField] private Card _firstCard;
        [SerializeField] private Card _secondCard;

        private Dictionary<CardData, IDisposable> _bottomCardSubscriptions = new Dictionary<CardData, IDisposable>();
        private CompositeDisposable _cardTableSubscriptions = new CompositeDisposable();

        private CardData _firstCardData;
        private CardData _secondCardData;

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
            StopObservingBottomCards();
            _firstCardData = null;
            _secondCardData = null;
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
            if (cardData.Card.Value == _firstCard && _firstCardData == null)
            {
                _firstCardData = cardData;
                OnFoundCardsUpdated();
            }
            else if (cardData.Card.Value == _secondCard)
            {
                if (_secondCardData == null)
                {
                    _secondCardData = cardData;
                    OnFoundCardsUpdated();
                }

                StartObservingBottomCard(cardData);
            }
        }

        private void OnRemovedCard(CardData cardData)
        {
            if (cardData.Card.Value == _firstCard && cardData == _firstCardData)
            {
                _cardsTable.TryGetFirstCard(_firstCard, out var card);
                _firstCardData = card;
                OnFoundCardsUpdated();

            }
            else if (cardData.Card.Value == _secondCard && cardData == _secondCardData)
            {
                _cardsTable.TryGetFirstCard(_secondCard, out var card);
                _secondCardData = card;
                OnFoundCardsUpdated();
            }

            StopObservingBottomCard(cardData);
        }

        private void OnClearCards()
        {
            StartTutorial();
        }

        private void StartObservingBottomCard(CardData cardData)
        {
            StopObservingBottomCard(cardData);

            IDisposable subscription = cardData.BottomCard.Subscribe(OnAnyBottomCardUpdated);

            _bottomCardSubscriptions.Add(cardData, subscription);
        }

        private void StopObservingBottomCard(CardData cardData)
        {
            if (_bottomCardSubscriptions.TryGetValue(cardData, out var subscription))
            {
                subscription?.Dispose();
                _bottomCardSubscriptions.Remove(cardData);
            }
        }

        private void StopObservingBottomCards()
        {
            foreach (var subscription in _bottomCardSubscriptions.Values)
            {
                subscription?.Dispose();
            }

            _bottomCardSubscriptions.Clear();
        }

        private void OnAnyBottomCardUpdated(CardData bottomCard)
        {
            if (bottomCard != null && bottomCard.Card.Value == _firstCard)
            {
                StopHandTutorial();
                _isFinished.Value = true;
            }
            else
            {
                bool hasAnyAppropriateBottomCard = _bottomCardSubscriptions.Keys.Any(cardData =>
                {
                    CardData bottomCard = cardData.BottomCard.Value;

                    if (bottomCard == null) return false;

                    return bottomCard.Card.Value == _firstCard;
                });

                if (hasAnyAppropriateBottomCard == false)
                {
                    if (_firstCardData != null && _secondCardData != null)
                    {
                        StartHandTutorial(_firstCardData.transform, _secondCardData.transform);
                    }

                    _isFinished.Value = false;
                }
            }
        }

        private void OnFoundCardsUpdated()
        {
            if (_firstCardData == null || _secondCardData == null)
            {
                StopHandTutorial();
                return;
            }

            StartHandTutorialDelayed(_firstCardData.transform, _secondCardData.transform);
        }
    }
}
