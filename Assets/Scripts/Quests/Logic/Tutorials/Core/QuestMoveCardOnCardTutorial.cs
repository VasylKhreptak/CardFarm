using System;
using System.Collections.Generic;
using System.Linq;
using Cards.Core;
using Cards.Data;
using Extensions;
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

        private Dictionary<CardDataHolder, IDisposable> _bottomCardSubscriptions = new Dictionary<CardDataHolder, IDisposable>();
        private CompositeDisposable _cardTableSubscriptions = new CompositeDisposable();

        private CardDataHolder _firstCardData;
        private CardDataHolder _secondCardData;

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

            IReadOnlyReactiveCollection<CardDataHolder> tableCards = _cardsTable.Cards;
            tableCards.ObserveAdd().Subscribe(addEvent => OnAddedCard(addEvent.Value)).AddTo(_cardTableSubscriptions);
            tableCards.ObserveRemove().Subscribe(removeEvent => OnRemovedCard(removeEvent.Value)).AddTo(_cardTableSubscriptions);
            tableCards.ObserveReset().Subscribe(_ => OnClearCards()).AddTo(_cardTableSubscriptions);
        }

        private void StopObservingCardsTable()
        {
            _cardTableSubscriptions?.Clear();
        }

        private void OnAddedCard(CardDataHolder cardData)
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

        private void OnRemovedCard(CardDataHolder cardData)
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

        private void StartObservingBottomCard(CardDataHolder cardData)
        {
            StopObservingBottomCard(cardData);

            IDisposable subscription = cardData.BottomCard.Subscribe(OnAnyBottomCardUpdated);

            _bottomCardSubscriptions.Add(cardData, subscription);
        }

        private void StopObservingBottomCard(CardDataHolder cardData)
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

        private void OnAnyBottomCardUpdated(CardDataHolder bottomCard)
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
                    CardDataHolder bottomCard = cardData.BottomCard.Value;

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

            TrySeparateFoundCards();

            StartHandTutorialDelayed(_firstCardData.transform, _secondCardData.transform);
        }

        private void TrySeparateFoundCards()
        {
            if (_firstCardData == null || _secondCardData == null) return;

            if (_firstCardData.IsSingleCard.Value == false)
            {
                _firstCardData.Separate();
                _firstCardData.Animations.JumpAnimation.PlayRandomly();
            }

            if (_secondCardData.IsSingleCard.Value == false)
            {
                _secondCardData.Separate();
                _secondCardData.Animations.JumpAnimation.PlayRandomly();
            }
        }

        protected override void OnRepeated()
        {
            // TrySeparateFoundCards();
        }
    }
}
