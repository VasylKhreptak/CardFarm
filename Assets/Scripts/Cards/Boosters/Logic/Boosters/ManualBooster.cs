using System.Collections.Generic;
using Cards.Boosters.Logic.Core;
using Cards.Core;
using Cards.Data;
using Cards.Logic.Spawn;
using Extensions;
using Table.Core;
using UnityEngine;
using Zenject;

namespace Cards.Boosters.Logic.Boosters
{
    public class ManualBooster : BoosterBaseLogic
    {
        [Header("Preferences")]
        [SerializeField] private List<Card> _cards;

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
                _cardSpawner.Spawn(cardToSpawn, position);
            }
        }

        private Card GetCardToSpawn()
        {
            return _cards[_cardData.TotalCards.Value - _cardData.LeftCards.Value];
        }
    }
}
