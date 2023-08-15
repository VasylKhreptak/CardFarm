using System;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace Graphics.Animations.Quests.RewardPanel
{
    public class RewardPanelHideAnimation : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private CanvasGroup _canvasGroup;

        [Header("General Preferences")]
        [SerializeField] private float _duration;

        [Header("Fade Preferences")]
        [SerializeField] private float _startAlpha;
        [SerializeField] private float _endAlpha;
        [SerializeField] private AnimationCurve _fadeCurve;

        [Header("Scale Preferences")]
        [SerializeField] private Vector3 _startScale;
        [SerializeField] private Vector3 _endScale;
        [SerializeField] private AnimationCurve _scaleCurve;

        private Sequence _sequence;

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
            Stop();

            _canvasGroup.alpha = _startAlpha;
            _rectTransform.localScale = _startScale;

            _sequence = DOTween.Sequence();

            _sequence
                .Append(_canvasGroup.DOFade(_endAlpha, _duration).SetEase(_fadeCurve))
                .Join(_rectTransform.DOScale(_endScale, _duration).SetEase(_scaleCurve))
                .OnComplete(() => onComplete?.Invoke())
                .Play();
        }

        public void Stop()
        {
            _sequence?.Kill();
        }
    }
}
