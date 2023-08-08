using System;
using Cards.Data;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Graphics.Logic.Animations
{
    public class CardShaker : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardDataHolder _cardData;

        [Header("Preferences")]
        [SerializeField] private float _interval = 10f;

        private IDisposable _intervalSubscription;

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _cardData = GetComponentInParent<CardDataHolder>(true);
        }

        private void OnEnable()
        {
            StartShaking();
        }

        private void OnDisable()
        {
            StopShaking();
        }

        #endregion

        private void StartShaking()
        {
            StopShaking();

            _intervalSubscription = Observable
                .Interval(TimeSpan.FromSeconds(_interval))
                .Subscribe(_ => Shake());
        }

        private void StopShaking()
        {
            _intervalSubscription?.Dispose();
        }

        private void Shake()
        {
            _cardData.Animations.ShakeAnimation.Play();
        }
    }
}
