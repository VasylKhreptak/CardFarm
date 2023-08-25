using System;
using System.Collections.Generic;
using Cards.Core;
using Cards.Data;
using Cards.Logic.Spawn;
using Extensions;
using Runtime.Commands;
using UniRx;
using UnityEngine;
using UnlockedCardPanel.Graphics.VisualElements;
using Zenject;

namespace Runtime.Map
{
    public class StarterCardsSpawner : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform _transform;

        [Header("Preferences")]
        [SerializeField] private List<Card> _cards = new List<Card>();
        [SerializeField] private float _delay;
        [SerializeField] private List<Transform> _spawnPoints = new List<Transform>();

        public event Action OnSpawnedAllCards;

        private IDisposable _delaySubscription;
        private IDisposable _cardPanelStateSubscription;

        private int _cardToSpawnIndex;

        private CardSpawner _cardSpawner;
        private GameRestartCommand _gameRestartCommand;
        private NewCardPanel _newCardPanel;

        [Inject]
        private void Constructor(CardSpawner cardSpawner,
            GameRestartCommand gameRestartCommand,
            NewCardPanel newCardPanel)
        {
            _cardSpawner = cardSpawner;
            _gameRestartCommand = gameRestartCommand;
            _newCardPanel = newCardPanel;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            _transform ??= GetComponent<Transform>();
            _spawnPoints.Resize(_cards.Count);
        }

        private void Awake()
        {
            _gameRestartCommand.OnExecute += OnRestart;
        }

        private void Start()
        {
            SpawnCards();
        }

        private void OnDestroy()
        {
            _gameRestartCommand.OnExecute -= OnRestart;
        }

        private void OnDisable()
        {
            _cardPanelStateSubscription?.Dispose();
            _gameRestartCommand.OnExecute -= OnRestart;
        }

        #endregion

        private void SpawnCards()
        {
            _cardPanelStateSubscription?.Dispose();

            _cardToSpawnIndex = 0;
            
            _delaySubscription?.Dispose();
            _delaySubscription = Observable.Timer(TimeSpan.FromSeconds(_delay)).Subscribe(_ =>
            {
                SpawnCardRecursive();
            });
        }

        private void SpawnCardRecursive()
        {
            if (_cardToSpawnIndex > _cards.Count - 1)
            {
                OnSpawnedAllCards?.Invoke();
                return;
            }

            Card cardToSpawn = _cards[_cardToSpawnIndex];

            Vector3 spawnPosition = _spawnPoints[_cardToSpawnIndex].position;

            CardData spawnedCard = _cardSpawner.Spawn(cardToSpawn, spawnPosition);

            spawnedCard.Animations.AppearAnimation.Play(() =>
            {
                _newCardPanel.Show(spawnedCard);
                
                _cardPanelStateSubscription?.Dispose();
                _cardPanelStateSubscription = _newCardPanel.IsActive.Where(x => x == false).Subscribe(_ =>
                {
                    _cardToSpawnIndex++;
                    SpawnCardRecursive();
                });
            });
        }

        private void OnRestart()
        {
            SpawnCards();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            foreach (Transform spawnPoint in _spawnPoints)
            {
                if (spawnPoint == null) continue;

                Gizmos.DrawSphere(spawnPoint.position, 0.5f);
            }
        }
    }
}
