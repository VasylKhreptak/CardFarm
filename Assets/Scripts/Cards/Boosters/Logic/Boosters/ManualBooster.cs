using System.Collections.Generic;
using Cards.Boosters.Logic.Core;
using Cards.Core;
using Cards.Logic.Spawn;
using UnityEngine;
using Zenject;

namespace Cards.Boosters.Logic.Boosters
{
    public class ManualBooster : BoosterBaseLogic
    {
        [Header("Preferences")]
        [SerializeField] private List<Card> _cards;

        private CardSpawner _cardSpawner;

        [Inject]
        private void Constructor(CardSpawner cardSpawner)
        {
            _cardSpawner = cardSpawner;
        }

        protected override void SpawnResultedCard()
        {
            Card cardToSpawn = GetCardToSpawn();

            if (cardToSpawn == Card.Coin)
            {
                _cardSpawner.SpawnCoinAndMove(_cardData.transform.position);
            }
            else
            {
                _cardSpawner.SpawnAndMove(cardToSpawn, _cardData.transform.position);
            }

            _cardData.BoosterCallabcks.OnSpawnedCard?.Invoke(cardToSpawn);
        }

        private Card GetCardToSpawn()
        {
            return _cards[_cardData.TotalCards.Value - _cardData.LeftCards.Value];
        }
    }
}
