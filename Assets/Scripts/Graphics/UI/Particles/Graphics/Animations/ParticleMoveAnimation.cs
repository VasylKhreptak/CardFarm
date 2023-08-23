using System;
using DG.Tweening;
using Extensions;
using Graphics.UI.Particles.Data;
using Providers.Graphics;
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

        private Camera _camera;

        [Inject]
        private void Constructor(CameraProvider cameraProvider)
        {
            _camera = cameraProvider.Value;
        }

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

        public void Play(Func<Vector3> targetWorldPositionGetter, Action onComplete = null)
        {
            Stop();

            Vector3 startAnchoredPosition = GetAnchoredPosition(_particleData.transform.position);
            
            float progress = 0;
            _animation = DOTween.To(() => progress, x => progress = x, 1, _moveDuration)
                .OnUpdate(() =>
                {
                    Vector3 targetAnchoredPosition = GetAnchoredPosition(targetWorldPositionGetter.Invoke());
                    Vector3 position = Vector3.Lerp(startAnchoredPosition, targetAnchoredPosition, progress);
                    _particleData.RectTransform.anchoredPosition3D = position;
                })
                .SetEase(_moveCurve)
                .OnComplete(() => onComplete?.Invoke())
                .Play();
        }

        public void Stop()
        {
            _animation.Kill();
        }

        public Vector2 GetAnchoredPosition(Vector3 worldPoint)
        {
            return _particleData.RectTransform.GetAnchoredPosition(_camera, worldPoint);
        }
    }
}
