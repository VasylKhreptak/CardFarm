using System.Collections.Generic;
using Cards.Boosters.Logic.Core;
using Cards.Core;
using Cards.Data;
using Cards.Logic.Spawn;
using Extensions;
using ScriptableObjects.Scripts.Cards.Recipes;
using Table.Core;
using UnityEngine;
using Zenject;

namespace Cards.Boosters.Logic.Boosters
{
    public class WeightedBooster : BoosterBaseLogic
    {
        [Header("Preferences")]
        [SerializeField] private List<CardWeight> _cards;
        [SerializeField] private float _cardMoveDuration = 0.5f;

        private CardSpawner _cardSpawner;
        private CardsTable _cardsTable;

        [Inject]
        private void Constructor(CardSpawner cardSpawner, CardsTable cardsTable)
        {
            _cardSpawner = cardSpawner;
            _cardsTable = cardsTable;
        }

        protected override void SpawnCard()
        {
            Card cardToSpawn = GetCardToSpawn();

            _cardData.BoosterCallabcks.OnSpawnedCard?.Invoke(cardToSpawn);

            if (_cardsTable.TryGetLowestGroupCard(cardToSpawn, out CardData lowestGroupCard))
            {
                Vector3 position = _cardData.transform.position;
                CardData spawnedCard = _cardSpawner.Spawn(cardToSpawn, position);
                spawnedCard.LinkTo(lowestGroupCard);
            }
            else
            {
                Vector3 position = GetRandomPosition();
                CardData spawnedCard = _cardSpawner.Spawn(cardToSpawn, _cardData.transform.position);
                spawnedCard.Animations.MoveAnimation.Play(position, _cardMoveDuration);
            }
        }

        private Card GetCardToSpawn()
        {
            return _cards.GetByWeight(x => x.Weight).Card;
        }
    }
}
