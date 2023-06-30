using System;
using Cards.Zones.BoosterBuyZone.Data;
using EditorTools.Validators.Core;
using TMPro;
using UniRx;
using UnityEngine;

namespace Cards.Zones.BoosterBuyZone.Graphics.VisualElements
{
    public class BoosterPriceText : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private BoosterBuyZoneData _cardData;
        [SerializeField] private TMP_Text _tmp;

        private IDisposable _priceSubscription;

        #region MonoBehaviour

        public void OnValidate()
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
            _priceSubscription = _cardData.BoosterPrice.Subscribe(SetPrice);
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
