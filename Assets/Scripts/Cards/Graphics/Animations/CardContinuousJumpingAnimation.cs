using System;
using Cards.Data;
using Cards.Graphics.Animations.Core;
using DG.Tweening;
using Extensions;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Graphics.Animations
{
    public class CardContinuousJumpingAnimation : CardAnimation, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        [Header("Preferences")]
        [SerializeField] private float _duration = 0.5f;
        [SerializeField] private float _interval = 0.2f;

        [Header("Jump Preferences")]
        [SerializeField] private float _jumpPower = 1f;
        [SerializeField] private int _numberOfJumps = 1;
        [SerializeField] private AnimationCurve _jumpCurve;

        private IDisposable _delaySubscription;

        private Sequence _sequence;

        public float Duration => _duration;

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _cardData = GetComponentInParent<CardData>(true);
        }

        protected virtual void OnDisable()
        {
            Stop();
        }

        #endregion

        public void Play(float delay, int loops = -1, Action onLoopPlay = null, Action onComplete = null)
        {
            _delaySubscription?.Dispose();
            _delaySubscription = Observable
                .Timer(TimeSpan.FromSeconds(delay))
                .Subscribe(_ => Play(loops, onLoopPlay, onComplete));
        }

        public void Play(int loops = -1, Action onLoopPlay = null, Action onComplete = null)
        {
            Vector3 jumpPosition = _cardData.transform.position;
            jumpPosition.y = _cardData.BaseHeight;

            _cardData.transform.position = jumpPosition;
            
            _cardData.UnlinkFromUpper();

            Stop();

            _sequence = DOTween.Sequence();

            _sequence
                .AppendCallback(() => onLoopPlay?.Invoke())
                .Join(_cardData.transform.DOJump(jumpPosition, _jumpPower, _numberOfJumps, _duration).SetEase(_jumpCurve))
                .OnStart(() =>
                {
                    _isPlaying.Value = true;
                })
                .OnUpdate(() =>
                {
                    Vector3 position = _cardData.transform.position;
                    _cardData.Height.Value = position.y;
                })
                .OnComplete(() =>
                {
                    _isPlaying.Value = false;
                    onComplete?.Invoke();
                })
                .OnKill(() =>
                {
                    _isPlaying.Value = false;
                })
                .SetLoops(loops, LoopType.Restart)
                .AppendInterval(_interval)
                .Play();
        }

        public void StopContinuously()
        {
            if (_sequence != null)
            {
                _sequence.OnStepComplete(Stop);
            }
        }

        public override void Stop()
        {
            _sequence?.Kill();
            _isPlaying.Value = false;
            _delaySubscription?.Dispose();
        }
    }
}
