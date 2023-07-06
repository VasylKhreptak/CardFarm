﻿using System;
using System.Collections.Generic;
using Cards.Core;
using Cards.Data;
using Cards.Logic.Spawn;
using Cards.Workers.Data;
using Extensions;
using ProgressLogic.Core;
using ScriptableObjects.Scripts.Cards.ResourceNodes;
using UniRx;
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

        private IDisposable _workersSubscription;

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
            StartObservingBottomCards();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            StopObservingBottomCards();
            StopObservingWorker();
        }

        #endregion

        private void StartObservingBottomCards()
        {
            StopObservingBottomCards();

            _cardData.Callbacks.onBottomCardsListUpdated += OnBottomCardsListUpdated;
        }

        private void StopObservingBottomCards()
        {
            _cardData.Callbacks.onBottomCardsListUpdated -= OnBottomCardsListUpdated;
        }

        private void OnBottomCardsListUpdated()
        {
            List<CardData> bottomCards = _cardData.BottomCards;

            if (bottomCards.Count == 1 && bottomCards[0].IsWorker)
            {
                StartProgress(_resourceNodeData.Recipe.Cooldown);
                StartObservingWorker();
                return;
            }

            StopProgress();
            StopObservingWorker();
        }

        protected override void OnProgressCompleted()
        {
            Card cardToSpawn = GetCardToSpawn();

            _cardSpawner.SpawnAndMove(cardToSpawn, _cardData.transform.position);

            StartProgress(_resourceNodeData.Recipe.Cooldown);
            StartObservingWorker();
        }

        private Card GetCardToSpawn()
        {
            return _resourceNodeData.Recipe.Result.Weights.GetByWeight(x => x.Weight).Card;
        }

        private void StartObservingWorker()
        {
            StopObservingWorker();

            WorkerData worker = _cardData.BottomCards[0] as WorkerData;
            _workersSubscription = worker.Efficiency.Subscribe(SetTimeScale);
        }

        private void StopObservingWorker()
        {
            _workersSubscription?.Dispose();
        }
    }
}
