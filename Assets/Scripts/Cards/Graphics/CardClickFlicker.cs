using System;
using Cards.Data;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Graphics
{
    public class CardClickFlicker : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;
        [SerializeField] private GameObject _flickerObject;

        [Header("Preferences")]
        [SerializeField] private float _flickerTime = 0.1f;

        private IDisposable _delayDisposable;

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
            _cardData.Callbacks.onClicked += Flicker;
            DisableFlickerLayer();
        }

        private void OnDisable()
        {
            _cardData.Callbacks.onClicked -= Flicker;
            _delayDisposable?.Dispose();
            DisableFlickerLayer();
        }

        #endregion

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
