using System;
using Cards.Data;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Graphics
{
    public class CardTouchFlicker : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;
        [SerializeField] private GameObject _flickerObject;

        [Header("Preferences")]
        [SerializeField] private float _flickerTime = 0.1f;

        private IDisposable _delayDisposable;
        private IDisposable _touchDisposable;

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _cardData = GetComponentInParent<CardData>(true);
        }

        private void OnEnable()
        {
            StartObservingTouch();
            DisableFlickerLayer();
        }

        private void OnDisable()
        {
            StopObservingTouch();
            _delayDisposable?.Dispose();
            DisableFlickerLayer();
        }

        #endregion

        private void StartObservingTouch()
        {
            _touchDisposable = _cardData.IsSelected.Where(x => x).Subscribe(_ => OnTouch());
        }

        private void StopObservingTouch()
        {
            _touchDisposable?.Dispose();
        }

        private void OnTouch()
        {
            Flicker();
        }

        private void Flicker()
        {
            _delayDisposable?.Dispose();
            EnableFlickerLayer();
            _delayDisposable = Observable.Timer(TimeSpan.FromSeconds(_flickerTime)).Subscribe(_ =>
            {
                _flickerObject.SetActive(false);
            });
        }

        private void EnableFlickerLayer() => _flickerObject.SetActive(true);

        private void DisableFlickerLayer() => _flickerObject.SetActive(false);
    }
}
