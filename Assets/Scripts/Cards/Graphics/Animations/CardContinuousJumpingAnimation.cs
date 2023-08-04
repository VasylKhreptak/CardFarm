using System;
using NaughtyAttributes;
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

        [Button()]
        public void PlayContinuous()
        {
            StopContinuous();

            _intervalDisposable = Observable
                .Interval(TimeSpan.FromSeconds(Duration + _jumpInterval))
                .DoOnSubscribe(() => Play(_cardData.transform.position))
                .Subscribe(_ =>
                {
                    Play(_cardData.transform.position);
                });
        }

        public void PlayContinuous(float duration)
        {
            PlayContinuous();

            _delayDisposable = Observable
                .Timer(TimeSpan.FromSeconds(duration))
                .Subscribe(_ =>
                {
                    StopContinuous();
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
