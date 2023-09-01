using System;
using Data.Player.Core;
using Data.Player.Experience;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Graphics.UI.Experience.Progress
{
    public class UpperExperienceProgress : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Slider _slider;

        [Header("Preferences")]
        [SerializeField] private float _duration;
        [SerializeField] private Ease _ease;
        [SerializeField] private float _showDuration = 0.5f;

        private IDisposable _experienceSubscription;

        private Sequence _sequence;

        private bool _isFilled;

        public event Action OnFilled;
        public event Action OnAfterPause;

        private ExperienceData _experienceData;

        [Inject]
        private void Constructor(PlayerData playerData)
        {
            _experienceData = playerData.ExperienceData;
        }

        #region MonoBehaviour

        private void OnEnable()
        {
            SetSliderValue(_experienceData.Progress.Value);
            StartObservingExperience();
        }

        private void OnDisable()
        {
            StopObservingExperience();
            KillSequence();
            _experienceSubscription?.Dispose();

            _isFilled = false;
        }

        #endregion

        private void StartObservingExperience()
        {
            StopObservingExperience();

            _experienceSubscription = _experienceData.Progress.Subscribe(OnExperienceProgressChanged);
        }

        private void StopObservingExperience()
        {
            _experienceSubscription?.Dispose();
        }

        private void KillSequence()
        {
            _sequence?.Kill();
        }

        private void OnExperienceProgressChanged(float progress)
        {
            if (_isFilled) return;

            if (Mathf.Approximately(progress, 1f))
            {
                _isFilled = true;
                SetSliderValueSmooth(1f, () => OnFilled?.Invoke(), () =>
                {
                    OnAfterPause?.Invoke();
                    SetSliderValue(0f);
                    _isFilled = false;
                    SetSliderValueSmooth(_experienceData.Progress.Value);
                });
            }
            else
            {
                SetSliderValueSmooth(progress);
            }
        }

        private void SetSliderValue(float value) => _slider.value = value;

        private void SetSliderValueSmooth(float value, Action onFilled = null, Action onAfterPause = null)
        {
            KillSequence();

            _sequence = DOTween.Sequence()
                .Append(_slider.DOValue(value, _duration).SetEase(_ease))
                .AppendCallback(() => onFilled?.Invoke())
                .AppendInterval(_showDuration)
                .AppendCallback(() => onAfterPause?.Invoke())
                .Play();
        }
    }
}
