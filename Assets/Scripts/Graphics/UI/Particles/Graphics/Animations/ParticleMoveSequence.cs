using System;
using DG.Tweening;
using Extensions;
using Graphics.UI.Particles.Data;
using Providers.Graphics;
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

        public void Play(Transform target, Action onComplete = null)
        {
            Stop();

            RectTransform rectTransform = _particleData.RectTransform;

            Vector3 startPosition = rectTransform.GetAnchoredPosition(_camera, rectTransform.position);

            rectTransform.localScale = _startScale;
            Tween scaleAnimation = rectTransform.DOScale(_endScale, _scaleDuration).SetEase(_scaleCurve);

            float moveProgress = 0f;
            Tween moveAnimation = DOTween
                .To(() => moveProgress, x => moveProgress = x, 1, _moveDuration)
                .SetEase(_moveCurve)
                .OnPlay(() =>
                {
                    Vector3 targetAnchoredPosition = rectTransform.GetAnchoredPosition(_camera, target.position);
                    rectTransform.anchoredPosition3D = Vector3.Lerp(startPosition, targetAnchoredPosition, moveProgress);
                    rectTransform.rotation = _camera.transform.rotation;
                })
                .OnUpdate(() =>
                {
                    Vector3 targetAnchoredPosition = rectTransform.GetAnchoredPosition(_camera, target.position);
                    rectTransform.anchoredPosition3D = Vector3.Lerp(startPosition, targetAnchoredPosition, moveProgress);
                    rectTransform.transform.forward = _camera.transform.forward;
                });

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
