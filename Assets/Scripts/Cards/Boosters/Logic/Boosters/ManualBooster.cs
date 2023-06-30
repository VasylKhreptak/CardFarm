using System.Collections.Generic;
using Cards.Boosters.Logic.Core;
using Cards.Core;
using Cards.Data;
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

            CardData spawnedCard = _cardSpawner.SpawnAndMove(cardToSpawn, _cardData.transform.position, null, false, true);

            spawnedCard.Animations.FlipAnimation.Play();

            _cardData.BoosterCallabcks.OnSpawnedCard?.Invoke(cardToSpawn);
        }

        private Card GetCardToSpawn()
        {
            return _cards[_cardData.TotalCards.Value - _cardData.LeftCards.Value];
        }
    }
}
