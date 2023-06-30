using System;
using Cards.Data;
using Cards.Graphics.Animations.Core;
using DG.Tweening;
using EditorTools.Validators.Core;
using Extensions;
using UnityEngine;

namespace Cards.Graphics.Animations
{
    public class CardJumpAnimation : CardAnimation, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        [Header("Preferences")]
        [SerializeField] private float _duration = 0.5f;

        [Header("Jump Preferences")]
        [SerializeField] private float _jumpPower = 1f;
        [SerializeField] private int _numberOfJumps = 1;
        [SerializeField] private Ease _jumpEase;

        private Tween _animation;

        #region MonoBehaviour

        public void OnValidate()
        {
            _cardData = GetComponentInParent<CardData>(true);
        }

        private void OnDisable()
        {
            Stop();
        }

        #endregion

        public void Play(Vector3 targetPosition, Action onComplete = null)
        {
            Play(targetPosition, _duration, onComplete);
        }

        public void Play(Vector3 targetPosition, float duration, Action onComplete = null)
        {
            targetPosition = ValidatePosition(targetPosition);

            _cardData.UnlinkFromUpper();

            Stop();

            _animation = _cardData.transform
                .DOJump(targetPosition, _jumpPower, _numberOfJumps, duration)
                .SetEase(_jumpEase)
                .OnStart(() =>
                {
                    _isPlaying.Value = true;
                })
                .OnComplete(() =>
                {
                    onComplete?.Invoke();
                    _isPlaying.Value = false;
                })
                .Play();
        }

        public void Stop()
        {
            _animation?.Kill();
            _isPlaying.Value = false;
        }

        private Vector3 ValidatePosition(Vector3 position)
        {
            Vector3 pos = position;
            pos.y = _cardData.BaseHeight;
            return pos;
        }
    }
}
