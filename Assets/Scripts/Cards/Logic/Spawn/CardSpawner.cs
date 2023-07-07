using System;
using Cards.Core;
using Cards.Data;
using Constraints.CardTable;
using Extensions;
using Table.Core;
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
        private CardsTable _cardsTable;

        [Inject]
        private void Constructor(CardFactory cardFactory, CardsTableBounds cardsTableBounds, CardsTable cardsTable)
        {
            _cardFactory = cardFactory;
            _cardsTableBounds = cardsTableBounds;
            _cardsTable = cardsTable;
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
            bool flip = true)
        {
            return SpawnAndMove(card, card, position, targetPosition, tryJoinToExistingGroup, jump, flip);
        }

        public CardData SpawnAndMove(
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

        public CardData SpawnAndMove(
            Card card,
            Card[] prioritizedCardsToJoin,
            Vector3 position,
            Vector3? targetPosition = null,
            bool tryJoinToExistingGroup = true,
            bool jump = true,
            bool flip = true)
        {

            CardData spawnedCard = Spawn(card, position);
            if (tryJoinToExistingGroup && _cardsTable.TryGetLowestUniqPrioritizedCompatibleGroupCard(spawnedCard, prioritizedCardsToJoin, out var lowestGroupCard))
            {
                spawnedCard.LinkTo(lowestGroupCard);
                return spawnedCard;
            }

            Bounds cardBounds = spawnedCard.Collider.bounds;
            cardBounds.center = position;
            Vector3 moveToPosition = targetPosition ?? _cardsTableBounds.GetRandomPositionInRange(cardBounds, _minRange, _maxRange);

            if (jump)
            {
                spawnedCard.Animations.JumpAnimation.Play(moveToPosition);
            }
            else
            {
                spawnedCard.Animations.MoveAnimation.Play(moveToPosition);
            }

            if (flip)
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
