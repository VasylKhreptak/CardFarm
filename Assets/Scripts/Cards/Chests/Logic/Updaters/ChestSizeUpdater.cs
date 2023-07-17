using System;
using Cards.Chests.Data;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Chests.Logic.Updaters
{
    public class ChestSizeUpdater : MonoBehaviour, IValidatable
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
            _chestData.Size.Value = count;
        }
    }
}
