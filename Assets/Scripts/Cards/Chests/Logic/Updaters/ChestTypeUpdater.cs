using System;
using System.Linq;
using Cards.Chests.Data;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Chests.Logic.Updaters
{
    public class ChestTypeUpdater : MonoBehaviour, IValidatable
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
            if (count > 0)
            {
                _chestData.ChestType.Value = _chestData.StoredCards.First().Card.Value;
            }
            else
            {
                _chestData.ChestType.Value = null;
            }
        }
    }
}
