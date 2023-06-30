using System.Collections.Generic;
using Cards.Boosters.Logic.Core;
using Cards.Core;
using Cards.Data;
using Cards.Logic.Spawn;
using Extensions;
using ScriptableObjects.Scripts.Cards.Recipes;
using UnityEngine;
using Zenject;

namespace Cards.Boosters.Logic.Boosters
{
    public class WeightedBooster : BoosterBaseLogic
    {
        [Header("Preferences")]
        [SerializeField] private List<CardWeight> _cards;

        private CardSpawner _cardSpawner;

        [Inject]
        private void Constructor(CardSpawner cardSpawner)
        {
            _cardSpawner = cardSpawner;
        }

        protected override void SpawnResultedCard()
        {
            Card cardToSpawn = GetCardToSpawn();

            _cardSpawner.SpawnAndMove(cardToSpawn, _cardData.transform.position);

            _cardData.BoosterCallabcks.OnSpawnedCard?.Invoke(cardToSpawn);
        }

        private Card GetCardToSpawn()
        {
            return _cards.GetByWeight(x => x.Weight).Card;
        }
    }
}
