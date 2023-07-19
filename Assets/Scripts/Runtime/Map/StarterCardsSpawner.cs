using System;
using System.Collections.Generic;
using Cards.Core;
using Cards.Logic.Spawn;
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

        private CompositeDisposable _subscriptions = new CompositeDisposable();

        private CardSpawner _cardSpawner;

        [Inject]
        private void Constructor(CardSpawner cardSpawner)
        {
            _cardSpawner = cardSpawner;
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

        private void OnDisable()
        {
            _subscriptions.Clear();
        }

        #endregion

        private void SpawnCards()
        {
            Observable.Timer(TimeSpan.FromSeconds(_delay)).Subscribe(_ =>
            {
                float delay = 0f;

                for (int i = 0; i < _cards.Count; i++)
                {
                    Card card = _cards[i];
                    Observable.Timer(TimeSpan.FromSeconds(delay)).Subscribe(_ =>
                    {
                        SpawnCard(card);
                    }).AddTo(_subscriptions);

                    delay += _interval;
                }
            }).AddTo(_subscriptions);
        }

        private void SpawnCard(Card card)
        {
            _cardSpawner.SpawnAndMove(card, _transform.position, tryJoinToExistingGroup: false);
        }
    }
}
