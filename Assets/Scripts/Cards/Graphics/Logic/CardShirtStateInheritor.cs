using System;
using Cards.Data;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Graphics.Logic
{
    public class CardShirtStateInheritor : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;
        [SerializeField] private GameObject _targetObject;

        [Header("Preferences")]
        [SerializeField] private bool _inverse;

        private IDisposable _subscription;

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _cardData = GetComponentInParent<CardData>(true);
            _targetObject ??= gameObject;
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

            _subscription = _cardData.CardShirt.IsEnabled.Subscribe(OnCardShirtStateUpdated);
        }

        private void StopObserving()
        {
            _subscription?.Dispose();
        }

        private void OnCardShirtStateUpdated(bool enabled)
        {
            _targetObject.SetActive(_inverse ? !enabled : enabled);
        }
    }
}
