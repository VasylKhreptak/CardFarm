﻿using System;
using System.Linq;
using Cards.Core;
using Cards.Logic.Spawn;
using Constraints.CardTable;
using Quests.Logic;
using Quests.Logic.Core;
using Table.Core;
using UniRx;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Cards.Orders
{
    public class OrderSpawner : MonoBehaviour
    {
        [Header("Preferences")]
        [SerializeField] private float _minSpawnInterval = 30f;
        [SerializeField] private float _maxSpawnInterval = 60f;
        [SerializeField] private Quest _spawnFromQuest = Quest.BuildAHouse;
        [SerializeField] private int _maxOrders = 5;

        private IDisposable _spawnSubscription;
        private CompositeDisposable _questSubscriptions = new CompositeDisposable();

        private QuestsManager _questsManager;
        private CardsTableSelector _cardsTableSelector;
        private CardSpawner _cardSpawner;
        private CardsTableBounds _cardsTableBounds;
        private CardsTable _cardsTable;

        [Inject]
        private void Constructor(
            QuestsManager questsManager,
            CardsTableSelector cardsTableSelector,
            CardSpawner cardSpawner,
            CardsTableBounds cardsTableBounds,
            CardsTable cardsTable)
        {
            _questsManager = questsManager;
            _cardsTableSelector = cardsTableSelector;
            _cardSpawner = cardSpawner;
            _cardsTableBounds = cardsTableBounds;
            _cardsTable = cardsTable;
        }

        #region MonoBehaviour

        private void OnEnable()
        {
            StartObservingQuests();
        }

        private void OnDisable()
        {
            StopObservingQuests();
        }

        #endregion

        private void StartObservingQuests()
        {
            StopObservingQuests();

            _questsManager.FinishedQuests.ObserveAdd().Subscribe(quest =>
            {
                if (quest.Value == _spawnFromQuest) StartSpawningOrders();
            }).AddTo(_questSubscriptions);

            _questsManager.FinishedQuests.ObserveRemove().Subscribe(quest =>
            {
                if (quest.Value == _spawnFromQuest) StopSpawningOrders();
            }).AddTo(_questSubscriptions);

            _questsManager.FinishedQuests.ObserveReset().Subscribe(_ => StopSpawningOrders()).AddTo(_questSubscriptions);
        }

        private void StopObservingQuests()
        {
            _questSubscriptions.Clear();
        }

        private void StartSpawningOrders()
        {
            StopSpawningOrders();

            _spawnSubscription = Observable
                .Timer(TimeSpan.FromSeconds(Random.Range(_minSpawnInterval, _maxSpawnInterval)))
                .Subscribe(_ =>
                {
                    if (_cardsTableSelector.GetCount(Card.Order) < _maxOrders)
                    {
                        SpawnOrder();
                    }

                    StartSpawningOrders();
                });
        }

        private void StopSpawningOrders()
        {
            _spawnSubscription?.Dispose();
        }

        private void SpawnOrder()
        {
            Bounds bounds = _cardsTable.ObservableCards.First().Collider.bounds;

            Vector3 randomPosition = _cardsTableBounds.GetRandomPoint(bounds);

            _cardSpawner.SpawnAndMove(Card.Order, randomPosition, randomPosition, false);
        }
    }
}