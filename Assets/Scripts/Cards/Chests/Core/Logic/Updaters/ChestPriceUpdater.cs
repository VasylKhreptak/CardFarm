using Cards.Chests.Core.Data;
using UniRx;
using UnityEngine;

namespace Cards.Chests.Core.Logic.Updaters
{
    public class ChestPriceUpdater : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private ChestCardData _cardData;

        private CompositeDisposable _subscriptions;

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

            _cardData.ItemsCount.Subscribe(_ => UpdatePrice()).AddTo(_subscriptions);
            _cardData.PriceForOneCard.Subscribe(_ => UpdatePrice()).AddTo(_subscriptions);
        }

        private void StopObserving()
        {
            _subscriptions.Clear();
        }

        private void UpdatePrice()
        {
            _cardData.Price.Value = _cardData.ItemsCount.Value * _cardData.PriceForOneCard.Value;
        }
    }
}
