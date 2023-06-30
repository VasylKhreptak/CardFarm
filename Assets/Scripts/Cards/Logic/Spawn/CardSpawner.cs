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

        public CardData SpawnAndMove(Card card, Vector3 position,
            Vector3? targetPosition = null,
            bool tryJoinToExistingGroup = true,
            bool jump = true,
            bool flip = true)
        {
            if (tryJoinToExistingGroup && _cardsTable.TryGetLowestCompatibleGroupCard(card, card, out var lowestGroupCard))
            {
                CardData spawnedCard = Spawn(card, position);
                spawnedCard.LinkTo(lowestGroupCard);
                return spawnedCard;
            }
            else
            {
                CardData spawnedCard = Spawn(card, position);
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
        }
    }
}
