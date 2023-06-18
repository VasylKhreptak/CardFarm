using System.Collections.Generic;
using Cards.Boosters.Logic.Core;
using Cards.Core;
using Cards.Data;
using Cards.Logic.Spawn;
using UnityEngine;
using Zenject;

namespace Cards.Boosters.Logic.BoosterBehaviours
{
    public class ManualBoosterLogic : BoosterBaseLogic
    {
        [Header("Preferences")]
        [SerializeField] private List<Card> _cards;

        private CardFactory _cardFactory;

        [Inject]
        private void Constructor(CardFactory cardFactory)
        {
            _cardFactory = cardFactory;
        }

        protected override void SpawnCard()
        {
            Card cardToSpawn = GetCardToSpawn();
            Vector3 position = GetRandomPosition();

            CardData spawnedCard = _cardFactory.Create(cardToSpawn);
            spawnedCard.transform.position = position;
        }

        private Card GetCardToSpawn()
        {
            return _cards[_cardData.TotalCards.Value - _cardData.LeftCards.Value];
        }
    }
}
