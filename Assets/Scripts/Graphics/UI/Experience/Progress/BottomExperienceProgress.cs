using System;
using Data.Player.Core;
using Data.Player.Experience;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Graphics.UI.Experience.Progress
{
    public class BottomExperienceProgress : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Slider _slider;
        [SerializeField] private UpperExperienceProgress _upperExperienceProgress;

        private IDisposable _experienceSubscription;

        private bool _isFilled;

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
            _upperExperienceProgress.OnAfterPause += OnUpperProgressCompleted;
        }

        private void OnDisable()
        {
            StopObservingExperience();
            _experienceSubscription?.Dispose();
            _upperExperienceProgress.OnAfterPause -= OnUpperProgressCompleted;

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

        private void OnExperienceProgressChanged(float progress)
        {
            if (_isFilled) return;

            if (Mathf.Approximately(progress, 1f))
            {
                _isFilled = true;
                SetSliderValue(progress);
            }
            else
            {
                SetSliderValue(progress);
            }
        }

        private void SetSliderValue(float value) => _slider.value = value;

        private void OnUpperProgressCompleted()
        {
            _isFilled = false;
            SetSliderValue(_experienceData.Progress.Value);
        }
    }
}
