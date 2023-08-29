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

        [Header("Move Preferences")]
        [SerializeField] private Vector2 _centerAnchoredPosition;
        [SerializeField] private AnimationCurve _moveCurve;
        
        [Header("Preferences")]
        [SerializeField] private float _duration;

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

        public void Play(Vector2 targetAnchoredPosition, Action onComplete = null)
        {
            Stop();

            _sequence = DOTween.Sequence();

            _sequence
                .Append(_canvasGroup.DOFade(_endAlpha, _duration).SetEase(_fadeCurve))
                .Join(_rectTransform.DOScale(_endScale, _duration).SetEase(_scaleCurve))
                .Join(_rectTransform.DOAnchorPos(targetAnchoredPosition, _duration).SetEase(_moveCurve))
                .OnPlay(() =>
                {
                    _isPlaying = true;
                })
                .OnComplete(() =>
                {
                    _isPlaying = false;
                    onComplete?.Invoke();
                })
                .OnKill(() =>
                {
                    _isPlaying = false;
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
