using System;
using DG.Tweening;
using Graphics.UI.Particles.Data;
using UnityEngine;
using Zenject;

namespace Graphics.UI.Particles.Graphics.Animations
{
    public class ParticleMoveSequence : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private ParticleData _particleData;

        [Header("Move Preferences")]
        [SerializeField] private float _moveDelay = 0.3f;
        [SerializeField] private float _moveDuration = 1f;
        [SerializeField] private AnimationCurve _moveCurve;

        [Header("Scale Preferences")]
        [SerializeField] private Vector3 _startScale;
        [SerializeField] private Vector3 _endScale;
        [SerializeField] private float _scaleDuration = 0.5f;
        [SerializeField] private AnimationCurve _scaleCurve;

        private Sequence _sequence;

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        private void OnDisable()
        {
            Stop();
        }

        public void Validate()
        {
            _particleData = GetComponentInParent<ParticleData>(true);
        }

        #endregion

        public void Play(Vector3 position, Action onComplete = null)
        {
            Stop();

            RectTransform rectTransform = _particleData.RectTransform;

            rectTransform.localScale = _startScale;
            Tween scaleAnimation = rectTransform.DOScale(_endScale, _scaleDuration).SetEase(_scaleCurve);
            Tween moveAnimation = rectTransform.DOMove(position, _moveDuration).SetEase(_moveCurve);

            _sequence = DOTween.Sequence();
            _sequence
                .Append(scaleAnimation)
                .AppendInterval(_moveDelay)
                .Append(moveAnimation)
                .OnComplete(() => onComplete?.Invoke())
                .Play();
        }

        public void Stop()
        {
            _sequence.Kill();
        }
    }
}
