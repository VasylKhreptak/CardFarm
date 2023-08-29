using System;
using Cards.Data;
using Cards.Graphics.Animations.Core;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace Cards.Graphics.Animations
{
    public class CardHeightAnimation : CardAnimation, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        [Header("Preferences")]
        [SerializeField] private float _duration = 0.3f;
        [SerializeField] private float _raisedHeight = 3f;
        [SerializeField] private AnimationCurve _curve;

        public float RaisedHeight => _raisedHeight;

        private Tween _heightAnimation;

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
            SetBaseHeight();
        }

        #endregion

        public void Play(Action onComplete = null) => Play(_raisedHeight, onComplete);

        public void Play(float height, Action onComplete = null)
        {
            Stop();

            _heightAnimation = _cardData.transform
                .DOMoveY(height, _duration)
                .SetEase(_curve)
                .OnPlay(() =>
                {
                    _isPlaying.Value = true;
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
                .Play();
        }

        public override void Stop()
        {
            _heightAnimation?.Kill();
        }

        [Button()]
        public void SetBaseHeight() => SetHeight(_cardData.BaseHeight);

        [Button()]
        public void SetRaisedHeight() => SetHeight(_raisedHeight);

        public void SetHeight(float height)
        {
            Vector3 position = _cardData.transform.position;
            position.y = height;
            _cardData.transform.position = position;
            _cardData.Height.Value = height;
        }

        private float GetHeight() => _cardData.transform.position.y;
    }
}
