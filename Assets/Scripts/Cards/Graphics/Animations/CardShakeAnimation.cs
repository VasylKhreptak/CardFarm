using System;
using Cards.Data;
using Cards.Graphics.Animations.Core;
using DG.Tweening;
using Extensions;
using UnityEngine;
using Zenject;

namespace Cards.Graphics.Animations
{
    public class CardShakeAnimation : CardAnimation, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        [Header("Preferences")]
        [SerializeField] private AnimationCurve _curve;
        [SerializeField] private float _duration = 0.5f;
        [SerializeField] private float _strength = 10f;
        [SerializeField] private int _vibrato = 10;
        [SerializeField] private float _randomness = 90f;
        [SerializeField] private ShakeRandomnessMode _randomnessMode = ShakeRandomnessMode.Full;

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

        public void Play(Action onComplete = null)
        {
            Play(_duration, onComplete);
        }

        public void Play(float duration, Action onComplete = null)
        {
            Stop();

            _cardData.transform.localRotation = Quaternion.identity;

            _tween = _cardData.transform.DOShakeRotation(duration, _strength, _vibrato, _randomness, true, _randomnessMode)
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
                .OnKill(() => _isPlaying.Value = false)
                .Play();
        }

        public override void Stop()
        {
            _tween?.Kill();
            _isPlaying.Value = false;
            _cardData.transform.localRotation = Quaternion.identity;
        }
    }
}
