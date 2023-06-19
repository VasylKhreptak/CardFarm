using DG.Tweening;
using UniRx;
using UnityEngine;

namespace ProgressLogic.Core
{
    public abstract class ProgressDependentObject : MonoBehaviour
    {
        protected FloatReactiveProperty _progress = new FloatReactiveProperty();

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
                })
                .OnKill(() =>
                {
                    _progress.Value = 0;
                })
                .SetEase(Ease.Linear)
                .Play();
        }

        protected void StopProgress()
        {
            _progressTween?.Kill();
            _progress.Value = 0;
        }

        protected abstract void OnProgressCompleted();
    }
}
