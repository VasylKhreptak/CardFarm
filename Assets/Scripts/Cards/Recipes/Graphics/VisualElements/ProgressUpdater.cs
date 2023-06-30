using System;
using ProgressLogic.Core;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Cards.Recipes.Graphics.VisualElements
{
    public class ProgressUpdater : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private ProgressDependentObject _progressDependentObject;
        [SerializeField] private Slider _slider;

        private IDisposable _progressSubscription;

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _slider = GetComponent<Slider>();
        }

        private void OnEnable()
        {
            StartObservingProgress();
        }

        private void OnDisable()
        {
            StopObservingProgress();
        }

        #endregion

        private void StartObservingProgress()
        {
            StopObservingProgress();
            _progressSubscription = _progressDependentObject.Progress.Subscribe(SetProgress);
        }

        private void StopObservingProgress()
        {
            _progressSubscription?.Dispose();
        }

        private void SetProgress(float progress)
        {
            _slider.value = progress;
        }
    }
}
