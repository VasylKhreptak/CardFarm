using System.Collections.Generic;
using Cards.Boosters.Logic.Core;
using Cards.Core;
using Cards.Logic.Spawn;
using UnityEngine;
using Zenject;

namespace Cards.Boosters.Logic.BoosterBehaviours
{
    public class ManualBoosterLogic : BoosterBaseLogic
    {
        [Header("Preferences")]
        [SerializeField] private List<Card> _cards;

        private CardSpawner _cardSpawner;

        [Inject]
        private void Constructor(CardSpawner cardSpawner)
        {
            _cardSpawner = cardSpawner;
        }

        protected override void SpawnCard()
        {
            Card cardToSpawn = GetCardToSpawn();
            Vector3 position = GetRandomPosition();

            _cardSpawner.Spawn(cardToSpawn, position);
        }

        private Card GetCardToSpawn()
        {
            return _cards[_cardData.TotalCards.Value - _cardData.LeftCards.Value];
        }
    }
}
