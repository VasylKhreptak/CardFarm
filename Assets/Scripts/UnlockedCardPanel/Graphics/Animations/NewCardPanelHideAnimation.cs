using System;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace UnlockedCardPanel.Graphics.Animations
{
    public class NewCardPanelHideAnimation : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private CanvasGroup _backgroundCanvasGroup;

        [Header("Preferences")]
        [SerializeField] private float _duration;

        [Header("Anchor Move Preferences")]
        [SerializeField] private AnimationCurve _moveCurve;

        [Header("Scale Preferences")]
        [SerializeField] private Vector3 _endScale;
        [SerializeField] private AnimationCurve _scaleCurve;

        [Header("Fade Preferences")]
        [SerializeField] private float _endAlpha;
        [SerializeField] private AnimationCurve _fadeCurve;

        [Header("Background Fade Preferences")]
        [SerializeField] private float _endBackgroundAlpha;
        [SerializeField] private AnimationCurve _backgroundFadeCurve;

        [Header("Hook Preferences")]
        [SerializeField] private Vector2 _hookOffset = new Vector2(0f, 250f);
        [SerializeField] private float _hookMoveDuration = 0.3f;
        [SerializeField] private AnimationCurve _hookMoveCurve;

        private Sequence _sequence;

        private bool _isPlaying;

        public bool IsPlaying => _isPlaying;

        #region MonoBehaviour

        private void OnValidate()
        {
            _rectTransform ??= GetComponent<RectTransform>();
            _canvasGroup ??= GetComponent<CanvasGroup>();
        }

        private void OnDisable()
        {
            Stop();
        }

        #endregion

        public void Play(Vector2 targetAnchoredPosition, Action onComplete = null, Action onMovedToHookPosition = null)
        {
            if (_isPlaying) return;

            Stop();

            _sequence = DOTween.Sequence();

            float totalDuration = _duration + _hookMoveDuration;
            
            Sequence moveSequence = DOTween.Sequence();

            moveSequence
                .Append(_rectTransform.DOAnchorPos(targetAnchoredPosition + _hookOffset, _hookMoveDuration).SetEase(_hookMoveCurve))
                .AppendCallback(() => onMovedToHookPosition?.Invoke())
                .Append(_rectTransform.DOAnchorPos(targetAnchoredPosition, _duration).SetEase(_moveCurve));

            _sequence
                .OnPlay(() => _isPlaying = true)
                .Join(_rectTransform.DOScale(_endScale, totalDuration).SetEase(_scaleCurve))
                .Join(_canvasGroup.DOFade(_endAlpha, totalDuration).SetEase(_fadeCurve))
                .Join(moveSequence)
                .Join(_backgroundCanvasGroup.DOFade(_endBackgroundAlpha, totalDuration).SetEase(_backgroundFadeCurve))
                .OnKill(() => _isPlaying = false)
                .OnComplete(() =>
                {
                    _isPlaying = false;
                    onComplete?.Invoke();
                })
                .Play();
        }

        [Button()]
        public void Stop()
        {
            _sequence.Kill();
        }
    }
}
