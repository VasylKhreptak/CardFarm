using Cards.Orders.Data;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Orders.Logic.Updaters
{
    public class OrderLeftCardsCountUpdater : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private OrderData _orderData;

        private CompositeDisposable _subscriptions = new CompositeDisposable();

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _orderData = GetComponentInParent<OrderData>(true);
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
            _orderData.CurrentCardsCount.Subscribe(_ => UpdateCount()).AddTo(_subscriptions);
            _orderData.TargetCardsCount.Subscribe(_ => UpdateCount()).AddTo(_subscriptions);
        }

        private void StopObserving()
        {
            _subscriptions.Clear();
        }

        private void UpdateCount()
        {
            _orderData.LeftCardsCount.Value = _orderData.TargetCardsCount.Value - _orderData.CurrentCardsCount.Value;
        }
    }
}
