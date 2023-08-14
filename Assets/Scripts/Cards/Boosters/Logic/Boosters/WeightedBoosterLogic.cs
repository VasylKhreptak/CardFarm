using System.Collections.Generic;
using Cards.Boosters.Logic.Core;
using Cards.Core;
using Cards.Logic.Spawn;
using Extensions;
using ScriptableObjects.Scripts.Cards.Recipes;
using UnityEngine;
using Zenject;

namespace Cards.Boosters.Logic.Boosters
{
    public class WeightedBoosterLogic : BoosterBaseLogic
    {
        [Header("Preferences")]
        [SerializeField] private List<CardWeight> _cards;

        private CardSpawner _cardSpawner;
        private CardsTable.Core.CardsTable _cardsTable;

        [Inject]
        private void Constructor(CardSpawner cardSpawner, CardsTable.Core.CardsTable cardsTable)
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
            return _cards.GetByWeight(x => x.Weight).Card;
        }
    }
}
