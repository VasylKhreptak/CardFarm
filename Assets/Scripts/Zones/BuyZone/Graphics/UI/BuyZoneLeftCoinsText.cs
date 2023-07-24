using System;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;
using Zones.BuyZone.Data;

namespace Zones.BuyZone.Graphics.UI
{
    public class BuyZoneLeftCoinsText : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private BuyZoneData _cardData;
        [SerializeField] private TMP_Text _tmp;

        private IDisposable _subscription;

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _tmp = GetComponent<TMP_Text>();
            _cardData = GetComponentInParent<BuyZoneData>(true);
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
            _subscription = _cardData.LeftCoinsCount.Subscribe(SetText);
        }

        private void StopObserving()
        {
            _subscription?.Dispose();
        }

        private void SetText(int value)
        {
            _tmp.text = value.ToString();
        }
    }
}
