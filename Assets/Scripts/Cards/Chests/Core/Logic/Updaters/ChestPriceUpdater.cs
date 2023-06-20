using System;
using Cards.Chests.Core.Data;
using UniRx;
using UnityEngine;

namespace Cards.Chests.Core.Logic.Updaters
{
    public class ChestPriceUpdater : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private ChestCardData _cardData;

        private IDisposable _storedCardsSubscriptions;

        #region MonoBehaviour

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

            _storedCardsSubscriptions = _cardData.StoredCards
                .ObserveAdd()
                .Select(x => x.Value.Price.Value)
                .Subscribe(AddToPrice);
        }

        private void StopObserving()
        {
            _storedCardsSubscriptions?.Dispose();
        }

        private void AddToPrice(int value)
        {
            _cardData.Price.Value += value;
        }
    }
}
