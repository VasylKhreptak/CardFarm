using System;
using Cards.Core;
using Cards.Data;
using Cards.Entities.Animals.Cattle.Data;
using Cards.Entities.Data;
using Cards.Logic.Spawn;
using Extensions;
using ScriptableObjects.Scripts.Cards.Cattle;
using Table.Core;
using UniRx;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Cards.Entities.Animals.Cattle.Logic
{
    public class CattleItemSpawner : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CattleCardData _cardData;

        [Header("Preferences")]
        [SerializeField] private CattleItemSpawnerData _data;

        [Header("Spawn Preferences")]
        [SerializeField] private float _minRange = 5f;
        [SerializeField] private float _maxRange = 7f;

        private IDisposable _intervalSubscription;
        private IDisposable _isCardSingleSubscription;

        private CardsTable _cardsTable;
        private CardSpawner _cardSpawner;

        [Inject]
        private void Constructor(CardsTable cardsTable, CardSpawner cardSpawner)
        {
            _cardsTable = cardsTable;
            _cardSpawner = cardSpawner;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            _cardData ??= GetComponentInParent<CattleCardData>();
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

            if (_cardsTable.TryGetLowestCompatibleGroupCard(cardToSpawn, cardToSpawn, out CardData lowestGroupCard))
            {
                Vector3 position = _cardData.transform.position;
                CardData spawnedCard = _cardSpawner.Spawn(cardToSpawn, position);
                spawnedCard.LinkTo(lowestGroupCard);
            }
            else
            {
                Vector3 position = GetRandomPosition();
                CardData spawnedCard = _cardSpawner.Spawn(cardToSpawn, _cardData.transform.position);
                spawnedCard.Animations.JumpAnimation.Play(position);
                spawnedCard.Animations.FlipAnimation.Play();
            }

            _cardData.CattleCallbacks.OnItemSpawned?.Invoke(cardToSpawn);
            _cardData.CattleCallbacks.OnItemSpawnedNoArgs?.Invoke();
        }

        private Vector3 GetRandomPosition()
        {
            float range = GetRange();

            Vector2 insideUnitCircle = Random.insideUnitCircle.normalized * range;

            Vector3 randomSphere = new Vector3(insideUnitCircle.x, 0f, insideUnitCircle.y);
            return _cardData.transform.position + randomSphere;
        }

        private float GetRange()
        {
            return Random.Range(_minRange, _maxRange);
        }
    }
}
