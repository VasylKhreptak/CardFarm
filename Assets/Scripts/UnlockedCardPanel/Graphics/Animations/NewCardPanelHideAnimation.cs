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

        [Header("Preferences")]
        [SerializeField] private float _duration;

        [Header("Anchor Move Preferences")]
        [SerializeField] private Vector2 _endAnchoredPosition;
        [SerializeField] private AnimationCurve _moveCurve;

        [Header("Scale Preferences")]
        [SerializeField] private Vector3 _endScale;
        [SerializeField] private AnimationCurve _scaleCurve;

        [Header("Fade Preferences")]
        [SerializeField] private float _endAlpha;
        [SerializeField] private AnimationCurve _fadeCurve;

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

        [Button()]
        public void Play(Action onComplete = null)
        {
            if (_isPlaying) return;

            Stop();

            _sequence = DOTween.Sequence();

            _sequence
                .OnPlay(() => _isPlaying = true)
                .Join(_rectTransform.DOScale(_endScale, _duration).SetEase(_scaleCurve))
                .Join(_canvasGroup.DOFade(_endAlpha, _duration).SetEase(_fadeCurve))
                .Join(_rectTransform.DOAnchorPos(_endAnchoredPosition, _duration).SetEase(_moveCurve))
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
