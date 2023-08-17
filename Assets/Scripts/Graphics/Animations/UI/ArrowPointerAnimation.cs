using System;
using DG.Tweening;
using NaughtyAttributes;
using UniRx;
using UnityEngine;

namespace Graphics.Animations.UI
{
    public class ArrowPointerAnimation : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private CanvasGroup _canvasGroup;

        [Header("Preferences")]
        [SerializeField] private float _generalDuration;
        [SerializeField] private float _playDelay;

        [Header("Fade Preferences")]
        [SerializeField] private float _startAlpha;
        [SerializeField] private float _endAlpha;
        [SerializeField] private float _fadeDuration;
        [SerializeField] private AnimationCurve _fadeCurve;

        [Header("Anchor Move Preferences")]
        [SerializeField] private Vector2 _startAnchorPosition;
        [SerializeField] private Vector2 _endAnchorPosition;
        [SerializeField] private AnimationCurve _moveCurve;

        [Header("Scale Preferences")]
        [SerializeField] private float _scaleDelay = 0.3f;
        [SerializeField] private Vector3 _startScale = Vector3.one;
        [SerializeField] private Vector3 _endScale = Vector3.one;
        [SerializeField] private AnimationCurve _scaleCurve;

        private IDisposable _delaySubscription;

        private Sequence _sequence;
        private Tween _fadeTween;

        #region MonoBehaviour

        private void OnValidate()
        {
            _rectTransform ??= GetComponent<RectTransform>();
        }

        private void OnDisable()
        {
            KillSequences();
            ResetValues();
            _delaySubscription?.Dispose();
        }

        #endregion


        [Button()]
        public void Play()
        {
            _delaySubscription?.Dispose();
            _delaySubscription = Observable.Timer(TimeSpan.FromSeconds(_playDelay)).Subscribe(_ =>
            {
                KillSequences();
                ResetValues();

                gameObject.SetActive(true);

                _fadeTween = _canvasGroup
                    .DOFade(_endAlpha, _fadeDuration)
                    .SetEase(_fadeCurve)
                    .Play();

                _sequence = DOTween.Sequence();

                _sequence
                    .Join(_rectTransform.DOAnchorPos(_endAnchorPosition, _generalDuration).SetEase(_moveCurve))
                    .Join(_rectTransform.DOScale(_endScale, _generalDuration).SetEase(_scaleCurve).SetDelay(_scaleDelay))
                    .SetLoops(-1, LoopType.Yoyo)
                    .Play();
            });
        }

        [Button()]
        public void Stop()
        {
            _delaySubscription?.Dispose();
            if (gameObject.activeSelf == false) return;

            KillSequences();

            _fadeTween = _canvasGroup
                .DOFade(_startAlpha, _fadeDuration)
                .SetEase(_fadeCurve)
                .OnComplete(() => gameObject.SetActive(false))
                .Play();
        }

        private void KillSequences()
        {
            _sequence?.Kill();
            _fadeTween?.Kill();
        }

        private void ResetValues()
        {
            _canvasGroup.alpha = _startAlpha;
            _rectTransform.anchoredPosition = _startAnchorPosition;
            _rectTransform.localScale = _startScale;
        }

        [Button()]
        private void AssignStartAnchorPosition()
        {
            _startAnchorPosition = _rectTransform.anchoredPosition;
        }

        [Button()]
        private void AssignStartScale()
        {
            _startScale = _rectTransform.localScale;
        }

        [Button()]
        private void AssignEndAnchorPosition()
        {
            _endAnchorPosition = _rectTransform.anchoredPosition;
        }

        [Button()]
        private void AssignEndScale()
        {
            _endScale = _rectTransform.localScale;
        }
    }
}
