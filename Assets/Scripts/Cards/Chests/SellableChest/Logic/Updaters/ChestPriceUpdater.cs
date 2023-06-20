using System;
using Cards.Chests.SellableChest.Data;
using UniRx;
using UnityEngine;

namespace Cards.Chests.SellableChest.Logic.Updaters
{
    public class ChestPriceUpdater : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private ChestSellableCardData _cardData;

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
