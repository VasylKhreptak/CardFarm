using System;
using Cards.Orders.Data;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Orders.Graphics.VisualElements
{
    public class OrderLeftCardsCountText : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private OrderData _orderData;
        [SerializeField] private TMP_Text _tmp;

        private IDisposable _leftCardsCountSubscription;

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _orderData = GetComponentInParent<OrderData>(true);
            _tmp = GetComponent<TMP_Text>();
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
            _leftCardsCountSubscription = _orderData.LeftCardsCount.Subscribe(SetText);
        }

        private void StopObserving()
        {
            _leftCardsCountSubscription?.Dispose();
        }

        private void SetText(int value)
        {
            _tmp.text = value.ToString();
        }
    }
}
