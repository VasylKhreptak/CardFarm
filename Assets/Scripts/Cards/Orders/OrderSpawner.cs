using System;
using System.Linq;
using Cards.Core;
using Cards.Logic.Spawn;
using Constraints;
using Quests.Data;
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
        private IDisposable _questAddSubscription;
        private IDisposable _questCompletionSubscription;

        private QuestsManager _questsManager;
        private CardsSelector _cardsSelector;
        private CardSpawner _cardSpawner;
        private UpperZoneBounds _upperZoneBounds;
        private CardsTable _cardsTable;

        [Inject]
        private void Constructor(
            QuestsManager questsManager,
            CardsSelector cardsSelector,
            CardSpawner cardSpawner,
            UpperZoneBounds upperZoneBounds,
            CardsTable cardsTable)
        {
            _questsManager = questsManager;
            _cardsSelector = cardsSelector;
            _cardSpawner = cardSpawner;
            _upperZoneBounds = upperZoneBounds;
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

            if (_questsManager.TryGetQuestData(_spawnFromQuest, out QuestData quest))
            {
                StartObservingQuestCompletion(quest);
            }
            else
            {
                _questAddSubscription = _questsManager.TotalQuests.ObserveAdd().Subscribe(addEvent =>
                {
                    if (addEvent.Value.Quest == _spawnFromQuest)
                    {
                        StartObservingQuestCompletion(addEvent.Value);
                        _questAddSubscription?.Dispose();
                    }
                });
            }
        }

        private void StartObservingQuestCompletion(QuestData quest)
        {
            StopObservingQuestCompletion();
            _questCompletionSubscription = quest.IsCompleted.Subscribe(isCompleted =>
            {
                if (isCompleted)
                {
                    StartSpawningOrders();
                }
                else
                {
                    StopSpawningOrders();
                }
            });
        }

        private void StopObservingQuestCompletion()
        {
            _questCompletionSubscription?.Dispose();
        }

        private void StopObservingQuests()
        {
            StopObservingQuestCompletion();
            _questAddSubscription?.Dispose();
        }

        private void StartSpawningOrders()
        {
            StopSpawningOrders();

            _spawnSubscription = Observable
                .Timer(TimeSpan.FromSeconds(Random.Range(_minSpawnInterval, _maxSpawnInterval)))
                .Subscribe(_ =>
                {
                    if (_cardsSelector.GetCount(Card.Order) < _maxOrders)
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
            if (_cardsTable.Cards.Count == 0) return;

            RectTransform rect = _cardsTable.Cards.First().RectTransform;

            Vector3 randomPosition = _upperZoneBounds.GetRandomPoint(rect);

            _cardSpawner.SpawnAndMove(Card.Order, randomPosition, randomPosition, false);
        }
    }
}
