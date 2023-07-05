using System;
using Cards.Logic.Spawn;
using Cards.Zones.BuyZone.Data;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Zones.BuyZone.Logic
{
    public class BuyZoneCardSpawner : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private BuyZoneData _cardData;

        private IDisposable _leftCoinsSubscription;

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
            _cardData = GetComponentInParent<BuyZoneData>(true);
        }

        private void OnEnable()
        {
            StartObservingLeftCoins();
        }

        private void OnDisable()
        {
            StopObservingLeftCoins();
        }

        #endregion

        private void StartObservingLeftCoins()
        {
            StopObservingLeftCoins();
            _leftCoinsSubscription = _cardData.LeftCoins.Subscribe(OnLeftCoinsUpdated);
        }

        private void StopObservingLeftCoins()
        {
            _leftCoinsSubscription?.Dispose();
        }

        private void OnLeftCoinsUpdated(int leftCoins)
        {
            if (leftCoins == 0)
            {
                SpawnCard();
            }
        }

        private void SpawnCard()
        {
            _cardSpawner.SpawnAndMove(_cardData.TargetCard.Value, _cardData.transform.position, _cardData.BoughtCardSpawnPoint.position);

            _cardData.BuyZoneCallbacks.onSpawnedCard?.Invoke();
        }
    }
}
