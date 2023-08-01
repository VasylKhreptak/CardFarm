using DG.Tweening;
using UniRx;
using UnityEngine;

namespace ProgressLogic.Core
{
    public abstract class ProgressDependentObject : MonoBehaviour
    {
        private FloatReactiveProperty _progress = new FloatReactiveProperty();

        public IReadOnlyReactiveProperty<float> Progress => _progress;

        private Tween _progressTween;

        #region MonoBehaviour

        protected virtual void OnDisable()
        {
            StopProgress();
        }

        #endregion

        protected void StartProgress(float duration)
        {
            StopProgress();

            float progress = 0;
            _progressTween = DOTween
                .To(() => progress, value => progress = value, 1f, duration)
                .OnStart(() =>
                {
                    _progress.Value = 0;
                })
                .OnUpdate(() => _progress.Value = progress)
                .OnComplete(() =>
                {
                    _progress.Value = 1;
                    OnProgressCompleted();
                    _progress.Value = 0;
                })
                .OnKill(() =>
                {
                    _progress.Value = 0;
                })
                .SetEase(Ease.Linear)
                .Play();
        }

        protected void SetTimeScale(float timeScale)
        {
            if (_progressTween == null) return;

            _progressTween.timeScale = timeScale;
        }

        public void SetPause(bool isPaused)
        {
            if (isPaused)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }

        public void Pause()
        {
            _progressTween?.Pause();
        }

        public void Resume()
        {
            _progressTween?.Play();
        }

        public void TogglePause()
        {
            _progressTween?.TogglePause();
        }

        protected void StopProgress()
        {
            _progressTween?.Kill();
            _progress.Value = 0;
            SetTimeScale(1f);
        }

        protected abstract void OnProgressCompleted();
    }
}
