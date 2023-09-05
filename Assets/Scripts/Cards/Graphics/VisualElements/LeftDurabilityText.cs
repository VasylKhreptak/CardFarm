using System;
using Cards.Data;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Graphics.VisualElements
{
    public class LeftDurabilityText : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;
        [SerializeField] private TMP_Text _tmp;

        [Header("Preferences")]
        [SerializeField] private string _format = "{0}x";

        private IDisposable _subscription;

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _cardData = GetComponentInParent<CardData>(true);
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
            StopObserving();

            _subscription = _cardData.Durability.Subscribe(OnDurabilityChanged);
        }

        private void StopObserving()
        {
            _subscription?.Dispose();
        }

        private void OnDurabilityChanged(int durability)
        {
            _tmp.text = string.Format(_format, durability);
        }
    }
}
