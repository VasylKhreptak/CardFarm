using Cards.Logic.Spawn;
using Cards.Zones.BuyZone.Data;
using UnityEngine;
using Zenject;

namespace Cards.Zones.BuyZone.Logic
{
    public class BuyZoneCardSpawner : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private BuyZoneData _cardData;

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
            StartObserving();
        }

        private void OnDisable()
        {
            StopObserving();
        }

        #endregion

        private void StartObserving()
        {
            StopObserving();
            _cardData.BuyZoneCallbacks.spawnCardCommand += SpawnCard;
        }

        private void StopObserving()
        {
            _cardData.BuyZoneCallbacks.spawnCardCommand -= SpawnCard;
        }

        private void SpawnCard()
        {
            _cardSpawner.SpawnAndMove(_cardData.TargetCard.Value, _cardData.transform.position, _cardData.BoughtCardSpawnPoint.position,
                appearAnimation:false);

            _cardData.BuyZoneCallbacks.onSpawnedCard?.Invoke();
        }
    }
}
