using System;
using System.Collections.Generic;
using System.Linq;
using Cards.Core;
using Cards.Data;
using Constraints.CardTable;
using Extensions;
using Tools.Bounds;
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

        public event Action<CardDataHolder> OnCardSpawned;
        public event Action OnCardSpawnedNonParameterized;

        private CardFactory _cardFactory;
        private CardsTableBounds _cardsTableBounds;
        private CardsTable.Core.CardsTable _cardsTable;
        private PlayingAreaTableBounds _playingAreaTableBounds;

        [Inject]
        private void Constructor(CardFactory cardFactory,
            CardsTableBounds cardsTableBounds,
            CardsTable.Core.CardsTable cardsTable,
            PlayingAreaTableBounds playingAreaTableBounds)
        {
            _cardFactory = cardFactory;
            _cardsTableBounds = cardsTableBounds;
            _cardsTable = cardsTable;
            _playingAreaTableBounds = playingAreaTableBounds;
        }

        public CardDataHolder Spawn(Card card, Vector3 position)
        {
            CardDataHolder spawnedCard = _cardFactory.Create(card);

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

        public CardDataHolder SpawnAndMove(Card card,
            Vector3 position,
            Vector3? targetPosition = null,
            bool tryJoinToExistingGroup = true,
            bool jump = true,
            bool flip = true)
        {
            return SpawnAndMove(card, card, position, targetPosition, tryJoinToExistingGroup, jump, flip);
        }

        public CardDataHolder SpawnAndMove(
            Card card,
            Card bottomCard,
            Vector3 position,
            Vector3? targetPosition = null,
            bool tryJoinToExistingGroup = true,
            bool jump = true,
            bool flip = true)
        {
            return SpawnAndMove(card, new[] { bottomCard }, position, targetPosition, tryJoinToExistingGroup, jump, flip);
        }

        public CardDataHolder SpawnAndMove(
            Card card,
            Card[] prioritizedCardsToJoin,
            Vector3 position,
            Vector3? targetPosition = null,
            bool tryJoinToExistingGroup = true,
            bool jump = true,
            bool flip = true)
        {

            CardDataHolder spawnedCard = Spawn(card, position);
            if (tryJoinToExistingGroup && _cardsTable.TryGetLowestUniqPrioritizedCompatibleGroupCard(spawnedCard, prioritizedCardsToJoin, out var lowestGroupCard))
            {
                spawnedCard.LinkTo(lowestGroupCard);
                return spawnedCard;
            }

            RectTransform spawnedCardRect = spawnedCard.RectTransform;
            Vector3 moveToPosition = targetPosition ?? _cardsTableBounds.GetRandomPositionInRange(spawnedCardRect, _minRange, _maxRange);
            List<RectTransform> cardsRect = _cardsTable.Cards.Select(x => x.RectTransform).ToList();
            cardsRect.Remove(spawnedCardRect);
            Vector3 freeSpacePosition = _playingAreaTableBounds.Bounds.GetClosestRandomPoint(cardsRect, spawnedCardRect, position);

            if (jump)
            {
                spawnedCard.Animations.JumpAnimation.Play(freeSpacePosition, () =>
                {
                    spawnedCard.Callbacks.onLanded?.Invoke();
                });
            }
            else
            {
                spawnedCard.Animations.MoveAnimation.Play(freeSpacePosition);
            }

            if (flip)
            {
                spawnedCard.Animations.FlipAnimation.Play();
            }

            return spawnedCard;
        }

        public CardDataHolder SpawnCoinAndMove(
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
