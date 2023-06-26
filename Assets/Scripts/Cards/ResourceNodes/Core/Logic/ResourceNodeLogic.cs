using System.Collections.Generic;
using Cards.Core;
using Cards.Data;
using Cards.Logic.Spawn;
using Extensions;
using ProgressLogic.Core;
using ScriptableObjects.Scripts.Cards.ResourceNodes;
using Table.Core;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Cards.ResourceNodes.Core.Logic
{
    public class ResourceNodeLogic : ProgressDependentObject
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        [FormerlySerializedAs("_resourceNodeRecipe")]
        [Header("Preferences")]
        [SerializeField] private ResourceNodeData _resourceNodeData;

        [Header("Spawn Preferences")]
        [SerializeField] private float _minRange = 5f;
        [SerializeField] private float _maxRange = 7f;

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
            OnBottomCardsListUpdated();
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
            StopObserving();

            _cardData.Callbacks.onBottomCardsListUpdated += OnBottomCardsListUpdated;
        }

        private void StopObserving()
        {
            _cardData.Callbacks.onBottomCardsListUpdated -= OnBottomCardsListUpdated;
        }

        private void OnBottomCardsListUpdated()
        {
            List<CardData> bottomCards = _cardData.BottomCards;

            if (bottomCards.Count == 1 && bottomCards[0].IsWorker)
            {
                StartProgress(_resourceNodeData.Recipe.Cooldown);
                return;
            }

            StopProgress();
        }

        protected override void OnProgressCompleted()
        {
            Card cardToSpawn = GetCardToSpawn();

            if (_cardsTable.TryGetLowestCompatibleGroupCard(cardToSpawn, cardToSpawn, out CardData lowestGroupCard))
            {
                Vector3 position = _cardData.transform.position;
                CardData spawnedCard = _cardSpawner.Spawn(cardToSpawn, position);
                spawnedCard.LinkTo(lowestGroupCard);
            }
            else
            {
                Vector3 position = GetRandomPosition();
                CardData spawnedCard = _cardSpawner.Spawn(cardToSpawn, _cardData.transform.position);
                spawnedCard.Animations.JumpAnimation.Play(position);
            }

            StartProgress(_resourceNodeData.Recipe.Cooldown);
        }

        private Vector3 GetRandomPosition()
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
            return _resourceNodeData.Recipe.Result.Weights.GetByWeight(x => x.Weight).Card;
        }
    }
}
