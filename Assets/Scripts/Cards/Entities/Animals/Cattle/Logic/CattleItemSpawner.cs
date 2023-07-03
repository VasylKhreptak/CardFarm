using System;
using Cards.Core;
using Cards.Entities.Animals.Cattle.Data;
using Cards.Logic.Spawn;
using Extensions;
using ScriptableObjects.Scripts.Cards.Recipes;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Entities.Animals.Cattle.Logic
{
    public class CattleItemSpawner : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CattleCardData _cardData;

        [Header("Preferences")]
        [SerializeField] private float _spawnInterval;
        [SerializeField] private CardWeight[] _result;

        private IDisposable _intervalSubscription;
        private IDisposable _isCardSingleSubscription;

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
            _cardData = GetComponentInParent<CattleCardData>(true);
        }

        private void OnEnable()
        {
            StarObserving();
        }

        private void OnDisable()
        {
            StopObserving();
        }

        #endregion

        private void StarObserving()
        {
            StopObserving();
            StartObservingIfCardSingle();
        }

        private void StopObserving()
        {
            StopSpawning();
            StopObservingIfCardSingle();
        }

        private void StartObservingIfCardSingle()
        {
            StopObservingIfCardSingle();

            _isCardSingleSubscription = _cardData.IsSingleCard.Subscribe(OnIsCardSingleValueChanged);
        }

        private void StopObservingIfCardSingle()
        {
            _isCardSingleSubscription?.Dispose();
        }

        private void OnIsCardSingleValueChanged(bool isSingle)
        {
            if (isSingle)
            {
                StartSpawning();
            }
            else
            {
                StopSpawning();
            }
        }

        private void StartSpawning()
        {
            StopSpawning();
            _intervalSubscription = Observable.Interval(TimeSpan.FromSeconds(_spawnInterval))
                .Subscribe(_ => SpawnItem());
        }

        private void StopSpawning()
        {
            _intervalSubscription?.Dispose();
        }

        private void SpawnItem()
        {
            Card cardToSpawn = _result.GetByWeight(x => x.Weight).Card;

            _cardSpawner.SpawnAndMove(cardToSpawn, _cardData.transform.position);

            _cardData.CattleCallbacks.OnItemSpawned?.Invoke(cardToSpawn);
            _cardData.CattleCallbacks.OnItemSpawnedNoArgs?.Invoke();
        }
    }
}
