using System;
using Cards.Data;
using TMPro;
using UniRx;
using UnityEngine;

namespace Cards.Graphics.CardVisualElements
{
    public class CardPriceText : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private SellableCardData _cardData;
        [SerializeField] private TMP_Text _tmp;

        private IDisposable _priceSubscription;

        #region MonoBehaviour

        private void OnValidate()
        {
            _tmp ??= GetComponent<TMP_Text>();
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
