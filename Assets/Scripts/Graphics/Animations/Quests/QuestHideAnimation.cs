using System;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace Graphics.Animations.Quests
{
    public class QuestHideAnimation : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform _transform;
        [SerializeField] private CanvasGroup _canvasGroup;

        [Header("Preferences")]
        [SerializeField] private float _duration;
        [SerializeField] private AnimationCurve _curve;

        [Header("Scale Preferences")]
        [SerializeField] private Vector3 _startScale;
        [SerializeField] private Vector3 _endScale;

        [Header("Fade Preferences")]
        [SerializeField] private float _startAlpha;
        [SerializeField] private float _endAlpha;

        private Sequence _sequence;

        #region MonoBehaviour

        private void OnValidate()
        {
            _transform ??= GetComponent<Transform>();
            _canvasGroup ??= GetComponent<CanvasGroup>();
        }

        private void OnDisable()
        {
            Stop();
        }

        #endregion

        [Button()]
        public void Play(Action onComplete = null)
        {
            Stop();

            _transform.localScale = _startScale;
            _canvasGroup.alpha = _startAlpha;

            _sequence = DOTween.Sequence();

            _sequence
                .Append(CreateScaleTween(_endScale, _curve, _duration))
                .Join(CreateFadeTween(_endAlpha, _curve, _duration))
                .OnComplete(() => onComplete?.Invoke())
                .Play();
        }

        private Tween CreateScaleTween(Vector3 endValue, AnimationCurve curve, float duration)
        {
            return _transform
                .DOScale(endValue, duration)
                .SetEase(curve);
        }

        private Tween CreateFadeTween(float endValue, AnimationCurve curve, float duration)
        {
            return _canvasGroup
                .DOFade(endValue, duration)
                .SetEase(curve);
        }

        public void Stop()
        {
            _sequence?.Kill();
        }
    }
}
