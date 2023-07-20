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

            _particleData.transform.DOMove(position, _moveDuration)
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
