using System;
using Cards.Data;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Graphics.VisualElements
{
    public class CardPriceText : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private SellableCardData _cardData;
        [SerializeField] private TMP_Text _tmp;

        [Header("Preferences")]
        // [SerializeField] private string _postfix = "$";

        private IDisposable _priceSubscription;

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
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
            // _tmp.text = price + _postfix;
            _tmp.text = price.ToString();
        }
    }
}
