using System;
using Cards.Data;
using Cards.Graphics.Animations.Core;
using DG.Tweening;
using Extensions;
using UnityEngine;
using Zenject;

namespace Cards.Graphics.Animations
{
    public class CardMoveAnimation : CardAnimation, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        [Header("Preferences")]
        [SerializeField] private AnimationCurve _curve;
        [SerializeField] private float _duration = 0.5f;

        private Tween _tween;

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
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
            _cardData.UnlinkFromUpper();

            Stop();
            targetPosition = ValidatePosition(targetPosition);

            _tween = _cardData.transform.DOMove(targetPosition, duration)
                .SetEase(_curve)
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
            _tween?.Kill();
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
