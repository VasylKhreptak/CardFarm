using System;
using System.Linq;
using Cards.Chests.Data;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Chests.Graphics.VisualElements
{
    public class ChestIconUpdater : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private ChestData _chestData;

        [Header("Preferences")]
        [SerializeField] private Sprite _defaultIcon;

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
            _chestData.Icon.Value = count > 0 ? _chestData.StoredCards.First().Icon.Value : _defaultIcon;
        }
    }
}
