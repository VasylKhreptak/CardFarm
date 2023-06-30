using System;
using Cards.Data;
using EditorTools.Validators.Core;
using TMPro;
using UniRx;
using UnityEngine;

namespace Cards.Graphics.VisualElements
{
    public class CardPriceText : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private SellableCardData _cardData;
        [SerializeField] private TMP_Text _tmp;

        private IDisposable _priceSubscription;

        #region MonoBehaviour

        public void OnValidate()
        {
            _tmp = GetComponent<TMP_Text>();
            _cardData = GetComponentInParent<SellableCardData>(true);
        }

        private void OnEnable()
        {
            StartObservingPrice();
        }

        private void OnDisable()
        {
            StopObservingPrice();
        }

        #endregion

        private void StartObservingPrice()
        {
            StopObservingPrice();
            _priceSubscription = _cardData.Price.Subscribe(SetPrice);
        }

        private void StopObservingPrice()
        {
            _priceSubscription?.Dispose();
        }

        private void SetPrice(int price)
        {
            _tmp.text = price.ToString();
        }
    }
}
