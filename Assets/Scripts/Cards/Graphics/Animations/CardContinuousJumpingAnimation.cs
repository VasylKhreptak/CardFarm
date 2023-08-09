using System;
using UniRx;
using UnityEngine;

namespace Cards.Graphics.Animations
{
    public class CardContinuousJumpingAnimation : CardJumpAnimation
    {
        [Header("Preferences")]
        [SerializeField] private float _jumpInterval = 0.5f;

        private IDisposable _intervalDisposable;
        private IDisposable _delayDisposable;

        #region MonoBehaviour

        protected override void OnDisable()
        {
            base.OnDisable();
            StopContinuous();
        }

        #endregion

        public void PlayContinuous(float duration = float.MaxValue, float delay = 0f)
        {
            StopContinuous();

            _delayDisposable?.Dispose();
            _delayDisposable = Observable.Timer(TimeSpan.FromSeconds(delay)).Subscribe(_ =>
            {
                _intervalDisposable?.Dispose();
                _intervalDisposable = Observable
                    .Interval(TimeSpan.FromSeconds(Duration + _jumpInterval))
                    .DoOnSubscribe(() => Play(_cardData.transform.position))
                    .Subscribe(_ =>
                    {
                        Play(_cardData.transform.position);
                    });

                if (Mathf.Approximately(duration, float.MaxValue) == false)
                {
                    _delayDisposable?.Dispose();
                    _delayDisposable = Observable
                        .Timer(TimeSpan.FromSeconds(duration))
                        .Subscribe(_ =>
                        {
                            StopContinuous();
                        });
                }
            });
        }

        public void StopContinuous()
        {
            _delayDisposable?.Dispose();
            _intervalDisposable?.Dispose();
        }

        public void StopAll()
        {
            StopContinuous();
            Stop();
        }
    }
}
