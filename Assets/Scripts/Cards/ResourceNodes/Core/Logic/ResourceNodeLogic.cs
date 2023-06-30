using System.Collections.Generic;
using Cards.Core;
using Cards.Data;
using Cards.Logic.Spawn;
using Extensions;
using ProgressLogic.Core;
using ScriptableObjects.Scripts.Cards.ResourceNodes;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Cards.ResourceNodes.Core.Logic
{
    public class ResourceNodeLogic : ProgressDependentObject, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        [FormerlySerializedAs("_resourceNodeRecipe")]
        [Header("Preferences")]
        [SerializeField] private ResourceNodeData _resourceNodeData;

        private CardSpawner _cardSpawner;

        [Inject]
        private void Constructor(CardSpawner cardSpawner)
        {
            _cardSpawner = cardSpawner;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _cardData = GetComponentInParent<CardData>(true);
        }

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

            _cardSpawner.SpawnAndMove(cardToSpawn, _cardData.transform.position);

            StartProgress(_resourceNodeData.Recipe.Cooldown);
        }

        private Card GetCardToSpawn()
        {
            return _resourceNodeData.Recipe.Result.Weights.GetByWeight(x => x.Weight).Card;
        }
    }
}
