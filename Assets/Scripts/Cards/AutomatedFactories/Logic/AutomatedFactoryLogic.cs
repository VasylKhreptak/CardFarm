using Cards.Core;
using Cards.Data;
using Cards.Logic.Spawn;
using Extensions;
using ProgressLogic.Core;
using ScriptableObjects.Scripts.Cards.AutomatedFactories.Recipes;
using Table.Core;
using UnityEngine;
using Zenject;

namespace Cards.AutomatedFactories.Logic
{
    public class AutomatedFactoryLogic : ProgressDependentObject
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;
        [SerializeField] private CardFactoryRecipes _recipes;

        [Header("Spawn Preferences")]
        [SerializeField] private float _minRange = 5f;
        [SerializeField] private float _maxRange = 7f;
        [SerializeField] private float _resultedCardMoveDuration = 0.5f;

        private CardsTable _cardsTable;
        private CardSpawner _cardSpawner;

        [Inject]
        private void Constructor(CardsTable cardsTable, CardSpawner cardSpawner)
        {
            _cardsTable = cardsTable;
            _cardSpawner = cardSpawner;
        }

        #region MonoBehaviour

        private void OnEnable()
        {
            StartObserving();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            StopObserving();
        }

        #endregion

        private void StartObserving()
        {
            _cardData.Callbacks.onBottomCardsListUpdated += OnBottomCardsUpdated;
        }

        private void StopObserving()
        {
            _cardData.Callbacks.onBottomCardsListUpdated -= OnBottomCardsUpdated;
        }

        protected override void OnProgressCompleted()
        {
        }

        private void OnBottomCardsUpdated()
        {

        }

        protected void SpawnResultedCard()
        {
            Card cardToSpawn = GetCardToSpawn();

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
                spawnedCard.Animations.MoveAnimation.Play(position, _resultedCardMoveDuration);
            }
        }

        protected Vector3 GetRandomPosition()
        {
            float range = GetRange();

            Vector2 insideUnitCircle = Random.insideUnitCircle.normalized * range;

            Vector3 randomSphere = new Vector3(insideUnitCircle.x, 0f, insideUnitCircle.y);
            return _cardData.transform.position + randomSphere;
        }

        private float GetRange()
        {
            return Random.Range(_minRange, _maxRange);
        }

        private Card GetCardToSpawn()
        {
            return Card.Coin;
        }
    }
}
