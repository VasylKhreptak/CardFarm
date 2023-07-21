using System;
using DG.Tweening;
using Graphics.UI.Particles.Data;
using UnityEngine;
using Zenject;

namespace Graphics.UI.Particles.Graphics.Animations
{
    public class ParticleMoveAnimation : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private ParticleData _particleData;

        [Header("Move Preferences")]
        [SerializeField] private float _moveDuration = 1f;
        [SerializeField] private AnimationCurve _moveCurve;

        private Tween _animation;

        public float Duration => _moveDuration;

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

        public void Play(Func<Vector3> targetPositionGetter, Action onComplete = null)
        {
            Stop();

            Vector3 startPosition = _particleData.transform.position;
            float progress = 0;
            _animation = DOTween.To(() => progress, x => progress = x, 1, _moveDuration)
                .OnUpdate(() =>
                {
                    Vector3 targetPosition = targetPositionGetter.Invoke();
                    Vector3 position = Vector3.Lerp(startPosition, targetPosition, progress);
                    _particleData.transform.position = position;
                })
                .SetEase(_moveCurve)
                .OnComplete(() => onComplete?.Invoke())
                .Play();
        }

        public void Stop()
        {
            _animation.Kill();
        }
    }
}
