﻿using System;
using Cards.Core;
using Cards.Data;
using CardsTable.ManualCardSelectors;
using Constraints.CardTable;
using Extensions;
using UnityEngine;
using Zenject;

namespace Cards.Logic.Spawn
{
    public class CardSpawner : MonoBehaviour
    {
        [Header("Preferences")]
        [SerializeField] private float _minRange = 5f;
        [SerializeField] private float _maxRange = 7f;

        [SerializeField] private Card[] _coinJoinablePrioritizedCards = new[]
        {
            Card.CoinChest, Card.Coin
        };

        public event Action<CardData> OnCardSpawned;
        public event Action OnCardSpawnedNonParameterized;

        private CardFactory _cardFactory;
        private CardsTableBounds _cardsTableBounds;
        private CardsTable.Core.CardsTable _cardsTable;
        private PlayingAreaTableBounds _playingAreaTableBounds;
        private InvestigatedCardsObserver _investigatedCardsObserver;

        [Inject]
        private void Constructor(CardFactory cardFactory,
            CardsTableBounds cardsTableBounds,
            CardsTable.Core.CardsTable cardsTable,
            PlayingAreaTableBounds playingAreaTableBounds,
            InvestigatedCardsObserver investigatedCardsObserver)
        {
            _cardFactory = cardFactory;
            _cardsTableBounds = cardsTableBounds;
            _cardsTable = cardsTable;
            _playingAreaTableBounds = playingAreaTableBounds;
            _investigatedCardsObserver = investigatedCardsObserver;
        }

        public CardData Spawn(Card card, Vector3 position)
        {
            CardData spawnedCard = _cardFactory.Create(card);

            spawnedCard.transform.localRotation = Quaternion.identity;
            spawnedCard.transform.localScale = Vector3.one;

            Vector3 spawnedCardPosition = spawnedCard.transform.position;

            spawnedCardPosition.x = position.x;
            spawnedCardPosition.z = position.z;
            spawnedCardPosition.y = spawnedCard.BaseHeight;

            spawnedCard.transform.position = spawnedCardPosition;

            OnCardSpawned?.Invoke(spawnedCard);
            OnCardSpawnedNonParameterized?.Invoke();

            return spawnedCard;
        }

        public CardData SpawnAndMove(Card card,
            Vector3 position,
            Vector3? targetPosition = null,
            bool tryJoinToExistingGroup = true,
            bool jump = true,
            bool flip = true,
            bool appearAnimation = true)
        {
            return SpawnAndMove(card, card, position, targetPosition, tryJoinToExistingGroup, jump, flip, appearAnimation);
        }

        public CardData SpawnAndMove(
            Card card,
            Card bottomCard,
            Vector3 position,
            Vector3? targetPosition = null,
            bool tryJoinToExistingGroup = true,
            bool jump = true,
            bool flip = true,
            bool appearAnimation = true)
        {
            return SpawnAndMove(card, new[] { bottomCard }, position, targetPosition, tryJoinToExistingGroup, jump, flip, appearAnimation);
        }

        public CardData SpawnAndMove(
            Card card,
            Card[] prioritizedCardsToJoin,
            Vector3 position,
            Vector3? targetPosition = null,
            bool tryJoinToExistingGroup = true,
            bool jump = true,
            bool flip = true,
            bool appearAnimation = true)
        {
            bool canPlayAppearAnimation = _investigatedCardsObserver.IsInvestigated(card) == false && appearAnimation;

            CardData spawnedCard = Spawn(card, position);
            if (tryJoinToExistingGroup && _cardsTable.TryGetLowestUniqPrioritizedCompatibleGroupCard(spawnedCard, prioritizedCardsToJoin, out var lowestGroupCard))
            {
                spawnedCard.LinkTo(lowestGroupCard);
                return spawnedCard;
            }

            RectTransform spawnedCardRect = spawnedCard.RectTransform;
            Vector3 moveToPosition = targetPosition ?? _playingAreaTableBounds.GetRandomPositionInRange(spawnedCardRect, _minRange, _maxRange);
            // List<RectTransform> cardsRect = _cardsTable.Cards.Select(x => x.RectTransform).ToList();
            // cardsRect.Remove(spawnedCardRect);
            // Vector3 freeSpacePosition = _playingAreaTableBounds.Bounds.GetClosestRandomPoint(cardsRect, spawnedCardRect, position);

            if (canPlayAppearAnimation)
            {
                spawnedCard.Animations.AppearAnimation.Play();
            }
            else if (jump)
            {
                spawnedCard.Animations.JumpAnimation.Play(moveToPosition, () =>
                {
                    spawnedCard.Callbacks.onLanded?.Invoke();
                });
            }
            else
            {
                spawnedCard.Animations.MoveAnimation.Play(moveToPosition);
            }

            if (flip && canPlayAppearAnimation == false)
            {
                spawnedCard.Animations.FlipAnimation.Play();
            }

            return spawnedCard;
        }

        public CardData SpawnCoinAndMove(
            Vector3 position,
            Vector3? targetPosition = null,
            bool tryJoinToExistingGroup = true,
            bool jump = true,
            bool flip = true)
        {
            return SpawnAndMove(Card.Coin, _coinJoinablePrioritizedCards, position, targetPosition, tryJoinToExistingGroup, jump, flip);
        }
    }
}
