using System;
using Cards.Zones.BuyZone.Data;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Zones.BuyZone.Graphics.VisualElements
{
    public class BuyZonePriceText : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private BuyZoneData _cardData;
        [SerializeField] private TMP_Text _tmp;

        private IDisposable _priceSubscription;

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _tmp = GetComponent<TMP_Text>();
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
