using System;
using Data.Player.Core;
using Data.Player.Experience;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Graphics.UI.Experience
{
    public class ExperienceProgress : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Slider _slider;

        [Header("Preferences")]
        [SerializeField] private float _duration = 0.5f;
        [SerializeField] private AnimationCurve _curve;

        private IDisposable _fillAmountSubscription;

        private Tween _tween;

        private ExperienceData _experienceData;

        [Inject]
        private void Constructor(PlayerData playerData)
        {
            _experienceData = playerData.ExperienceData;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            _slider ??= GetComponent<Slider>();
        }

        private void OnEnable()
        {
            SetSliderValue(_experienceData.FillAmount.Value);
            StartObserving();
        }

        private void OnDisable()
        {
            StopObserving();
            StopUpdatingSliderValue();
        }

        #endregion

        private void StartObserving()
        {
            StopObserving();
            _fillAmountSubscription = _experienceData.FillAmount
                .Subscribe(SetSliderValueSmooth);
        }

        private void StopObserving()
        {
            _fillAmountSubscription?.Dispose();
        }

        private void SetSliderValueSmooth(float value)
        {
            StopUpdatingSliderValue();

            _tween = DOTween
                .To(GetSliderValue, SetSliderValue, value, _duration)
                .SetEase(_curve)
                .Play();
        }

        private float GetSliderValue() => _slider.value;

        private void SetSliderValue(float value) => _slider.value = value;

        private void StopUpdatingSliderValue()
        {
            _tween?.Kill();
        }
    }
}
