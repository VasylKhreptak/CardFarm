using System;
using System.Collections.Generic;
using Cards.Core;
using Cards.Data;
using Cards.Logic.Spawn;
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
        }

        private void Start()
        {
            SpawnCards();
        }

        private void OnEnable()
        {
            _gameRestartCommand.OnExecute += OnRestart;
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
                    Observable.Timer(TimeSpan.FromSeconds(delay)).Subscribe(_ =>
                    {
                        CardData spawnedCard = SpawnCard(card);
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

        private CardData SpawnCard(Card card)
        {
            return _cardSpawner.SpawnAndMove(card, _transform.position, tryJoinToExistingGroup: false);
        }

        private void OnRestart()
        {
            _subscriptions.Clear();
            SpawnCards();
        }
    }
}
