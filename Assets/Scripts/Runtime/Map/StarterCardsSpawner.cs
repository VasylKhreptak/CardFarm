using System;
using System.Collections.Generic;
using CameraManagement.CameraAim.Core;
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

        [Header("Camera Distance Preferences")]
        [SerializeField] private float _startDistance;
        [SerializeField] private float _endDistance;

        public event Action OnSpawnedAllCards;

        private IDisposable _delaySubscription;
        private IDisposable _cardPanelStateSubscription;

        private int _cardToSpawnIndex;

        private CardSpawner _cardSpawner;
        private GameRestartCommand _gameRestartCommand;
        private NewCardPanel _newCardPanel;
        private CameraAimer _cameraAimer;

        [Inject]
        private void Constructor(CardSpawner cardSpawner,
            GameRestartCommand gameRestartCommand,
            NewCardPanel newCardPanel,
            CameraAimer cameraAimer)
        {
            _cardSpawner = cardSpawner;
            _gameRestartCommand = gameRestartCommand;
            _newCardPanel = newCardPanel;
            _cameraAimer = cameraAimer;
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
                _cardPanelStateSubscription?.Dispose();
                _cameraAimer.Aim(_transform, _endDistance);
                OnSpawnedAllCards?.Invoke();
                return;
            }

            Card cardToSpawn = _cards[_cardToSpawnIndex];

            Vector3 spawnPosition = _spawnPoints[_cardToSpawnIndex].position;

            _newCardPanel.MarkAsActive();

            CardData spawnedCard = _cardSpawner.Spawn(cardToSpawn, _transform.position);

            spawnedCard.transform.localRotation = Quaternion.Euler(-180, 0, 0);
            spawnedCard.NewCardShirtStateUpdater.UpdateCullState();

            _cameraAimer.Aim(spawnedCard.transform, _startDistance);

            spawnedCard.Animations.JumpAnimation.Play(spawnPosition, () =>
            {
                _newCardPanel.Show(spawnedCard, onStart: () =>
                {
                    _cameraAimer.StopAiming();
                });

                _cardPanelStateSubscription?.Dispose();
                _cardPanelStateSubscription = _newCardPanel.IsActive
                    .Where(x => x == false)
                    .Subscribe(_ =>
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
