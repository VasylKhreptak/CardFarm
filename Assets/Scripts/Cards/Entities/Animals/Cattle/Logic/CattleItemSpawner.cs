using System;
using Cards.Core;
using Cards.Data;
using Cards.Entities.Animals.Cattle.Data;
using Cards.Logic.Spawn;
using Extensions;
using ScriptableObjects.Scripts.Cards.Cattle;
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
        [SerializeField] private CattleItemSpawnerData _data;

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
            StopInterval();
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
                StartInterval();
            }
            else
            {
                StopInterval();
            }
        }

        private void StartInterval()
        {
            StopInterval();

            _intervalSubscription = Observable
                .Interval(TimeSpan.FromSeconds(_data.Recipe.Cooldown))
                .Subscribe(_ => OnIntervalTick());
        }

        private void StopInterval()
        {
            _intervalSubscription?.Dispose();
        }

        private void OnIntervalTick()
        {
            TrySpawnItem();
        }

        private void TrySpawnItem()
        {
            CattleCardWeight cattleCard = _data.Recipe.Weights.GetByWeight(x => x.Weight);

            if (MathfExtensions.Probability(cattleCard.SpawnChance) == false) return;

            Card cardToSpawn = cattleCard.Card;

            CardData spawnedCard = _cardSpawner.SpawnAndMove(cardToSpawn, _cardData.transform.position,
                null, true, true);
            spawnedCard.Animations.FlipAnimation.Play();

            _cardData.CattleCallbacks.OnItemSpawned?.Invoke(cardToSpawn);
            _cardData.CattleCallbacks.OnItemSpawnedNoArgs?.Invoke();
        }
    }
}
