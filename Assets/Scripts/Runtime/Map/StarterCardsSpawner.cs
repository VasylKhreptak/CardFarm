using System;
using System.Collections.Generic;
using Cards.Core;
using Cards.Data;
using Cards.Logic.Spawn;
using Extensions;
using Runtime.Commands;
using UniRx;
using UnityEngine;
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
        [SerializeField] private float _interval;
        [SerializeField] private List<Transform> _spawnPoints = new List<Transform>();

        public event Action OnSpawnedAllCards;

        private CompositeDisposable _subscriptions = new CompositeDisposable();

        private CardSpawner _cardSpawner;
        private GameRestartCommand _gameRestartCommand;

        [Inject]
        private void Constructor(CardSpawner cardSpawner, GameRestartCommand gameRestartCommand)
        {
            _cardSpawner = cardSpawner;
            _gameRestartCommand = gameRestartCommand;
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
            _subscriptions.Clear();
            _gameRestartCommand.OnExecute -= OnRestart;
        }

        #endregion

        private void SpawnCards()
        {
            _subscriptions.Clear();
            Observable.Timer(TimeSpan.FromSeconds(_delay)).Subscribe(_ =>
            {
                float delay = 0f;

                float moveDuration = 0;

                for (int i = 0; i < _cards.Count; i++)
                {
                    Card card = _cards[i];
                    Vector3 position = _spawnPoints[i].position;
                    Observable.Timer(TimeSpan.FromSeconds(delay)).Subscribe(_ =>
                    {
                        CardData spawnedCard = _cardSpawner.Spawn(card, _transform.position);
                        spawnedCard.Animations.JumpAnimation.Play(position);
                        spawnedCard.Animations.FlipAnimation.Play();

                        moveDuration = spawnedCard.Animations.JumpAnimation.Duration;
                    }).AddTo(_subscriptions);

                    delay += _interval;
                }

                Observable.Timer(TimeSpan.FromSeconds(delay + moveDuration)).Subscribe(_ =>
                {
                    OnSpawnedAllCards?.Invoke();
                }).AddTo(_subscriptions);

            }).AddTo(_subscriptions);
        }

        private void OnRestart()
        {
            _subscriptions.Clear();
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
