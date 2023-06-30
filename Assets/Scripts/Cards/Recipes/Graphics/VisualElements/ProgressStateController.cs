using System;
using ProgressLogic.Core;
using UniRx;
using UnityEngine;

namespace Cards.Recipes.Graphics.VisualElements
{
    public class ProgressStateController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private ProgressDependentObject _progressDependentObject;
        [SerializeField] private GameObject _progressObject;

        private IDisposable _progressSubscription;

        #region MonoBehaviour

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
            _progressSubscription = _progressDependentObject.Progress.Subscribe(OnProgressChanged);
        }

        private void StopObservingProgress()
        {
            _progressSubscription?.Dispose();
        }

        private void OnProgressChanged(float progress)
        {
            _progressObject.SetActive(progress > 0);
        }
    }
}
