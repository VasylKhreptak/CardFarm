using System;
using Cards.Core;
using Cards.Data;
using UnityEngine;
using Zenject;

namespace Cards.Logic.Spawn
{
    public class CardSpawner : MonoBehaviour
    {
        private CardFactory _cardFactory;

        public event Action<CardData> OnCardSpawned;
        public event Action OnCardSpawnedNonParameterized;

        [Inject]
        private void Constructor(CardFactory cardFactory)
        {
            _cardFactory = cardFactory;
        }

        public CardData Spawn(Card card)
        {
            CardData spawnedCard = _cardFactory.Create(card);

            spawnedCard.transform.localRotation = Quaternion.identity;
            spawnedCard.transform.localScale = Vector3.one;

            OnCardSpawned?.Invoke(spawnedCard);
            OnCardSpawnedNonParameterized?.Invoke();

            return spawnedCard;
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
    }
}
