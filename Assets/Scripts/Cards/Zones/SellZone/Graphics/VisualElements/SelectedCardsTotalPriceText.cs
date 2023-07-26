using System;
using Cards.Zones.SellZone.Data;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Zones.SellZone.Graphics.VisualElements
{
    public class SelectedCardsTotalPriceText : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private SellZoneData _sellZoneData;
        [SerializeField] private TMP_Text _tmp;

        private IDisposable _subscription;

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _sellZoneData = GetComponentInParent<SellZoneData>(true);
            _tmp ??= GetComponent<TMP_Text>();
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
            _subscription = _sellZoneData.SelectedCardsTotalPrice.Subscribe(SetText);
        }

        private void StopObserving()
        {
            _subscription?.Dispose();
        }

        private void SetText(int totalPrice)
        {
            _tmp.text = totalPrice == 0 ? "" : totalPrice.ToString();
        }
    }
}
