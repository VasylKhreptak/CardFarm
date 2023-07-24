using UnityEngine;
using Zenject;
using Zones.BuyZone.Data;

namespace Zones.BuyZone.Logic
{
    public class BuyZoneCardSpawner : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private BuyZoneData _buyZoneData;

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _buyZoneData = GetComponentInParent<BuyZoneData>(true);
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
            _buyZoneData.Callbacks.OnCollectedAllCoins += SpawnCard;
        }

        private void StopObserving()
        {
            _buyZoneData.Callbacks.OnCollectedAllCoins -= SpawnCard;
        }

        private void SpawnCard()
        {
            Debug.Log("Spawned Card");
        }
    }
}
