using System;
using EditorTools.Validators.Core;
using ProgressLogic.Core;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Cards.Recipes.Graphics.VisualElements
{
    public class ProgressUpdater : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private ProgressDependentObject _progressDependentObject;
        [SerializeField] private Slider _slider;

        private IDisposable _progressSubscription;

        #region MonoBehaviour

        public void OnValidate()
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
