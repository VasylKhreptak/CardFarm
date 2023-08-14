using System.Collections.Generic;
using Cards.Boosters.Logic.Core;
using Cards.Core;
using Cards.Logic.Spawn;
using Data;
using Extensions;
using NaughtyAttributes;
using ScriptableObjects.Scripts.Cards.Recipes;
using UnityEngine;
using Zenject;

namespace Cards.Boosters.Logic.Boosters
{
    public class WeightedBoosterLogic : BoosterBaseLogic
    {
        [Header("Preferences")]
        [SerializeField] private bool _usePseudoRandom = true;
        [SerializeField, ShowIf(nameof(_usePseudoRandom))] private Card _firstCard;
        [SerializeField] private List<CardWeight> _cards;

        private const string KEY = "WeightedBooster";

        private CardSpawner _cardSpawner;
        private TemporaryDataStorage _temporaryDataStorage;

        [Inject]
        private void Constructor(CardSpawner cardSpawner,
            TemporaryDataStorage temporaryDataStorage)
        {
            _cardSpawner = cardSpawner;
            _temporaryDataStorage = temporaryDataStorage;
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
            if (_usePseudoRandom)
            {
                TemporaryData<bool> spawnedFirstCard;

                _temporaryDataStorage.GetValue(KEY, new TemporaryData<bool>(false), out var foundData);
                {
                    spawnedFirstCard = foundData as TemporaryData<bool>;
                }

                if (!spawnedFirstCard.Value)
                {
                    spawnedFirstCard.Value = true;
                    _temporaryDataStorage.SetValue(KEY, spawnedFirstCard);
                    return _firstCard;
                }
            }

            return _cards.GetByWeight(x => x.Weight).Card;
        }
    }
}
