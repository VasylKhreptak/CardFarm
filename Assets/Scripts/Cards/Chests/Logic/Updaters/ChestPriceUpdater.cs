using System;
using System.Linq;
using Cards.Chests.Data;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Chests.Logic.Updaters
{
    public class ChestPriceUpdater : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private ChestData _chestData;

        private IDisposable _storedCardsCountSubscription;

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _chestData = GetComponentInParent<ChestData>(true);
        }

        private void Awake()
        {
            StartObserving();
        }

        private void OnDestroy()
        {
            StopObserving();
        }

        #endregion

        private void StartObserving()
        {
            _storedCardsCountSubscription = _chestData.StoredCards
                .ObserveCountChanged()
                .DoOnSubscribe(() => OnStoredCardsCountUpdated(_chestData.StoredCards.Count))
                .Subscribe(OnStoredCardsCountUpdated);
        }

        private void StopObserving()
        {
            _storedCardsCountSubscription?.Dispose();
        }

        private void OnStoredCardsCountUpdated(int count)
        {
            int price = 0;

            if (count > 0)
            {
                price = count * _chestData.StoredCards.First().Price.Value;
            }

            _chestData.Price.Value = price;
        }
    }
}
