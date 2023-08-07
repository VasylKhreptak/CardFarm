using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace Graphics.Animations.Quests
{
    public class QuestShowAnimation : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private CanvasGroup _canvasGroup;

        [Header("Move Preferences")]
        [SerializeField] private Vector2 _startAnchoredPosition;
        [SerializeField] private AnimationCurve _startAnchoredPositionCurve;
        [SerializeField] private Vector2 _showAnchoredPosition;
        [SerializeField] private Vector2 _endAnchoredPosition;
        [SerializeField] private AnimationCurve _endAnchoredPositionCurve;
        [SerializeField] private float _moveToShowDuration = 1f;
        [SerializeField] private float _showDuration;
        [SerializeField] private float _moveToEndDuration;

        [Header("Scale Preferences")]
        [SerializeField] private Vector3 _startScale;
        [SerializeField] private AnimationCurve _startScaleCurve;
        [SerializeField] private Vector3 _showScale;
        [SerializeField] private Vector3 _endScale;
        [SerializeField] private AnimationCurve _endScaleCurve;

        [Header("Fade Preferences")]
        [SerializeField] private float _startAlpha;
        [SerializeField] private float _showAlpha;
        [SerializeField] private float _endAlpha;
        [SerializeField] private AnimationCurve _startFadeCurve;
        [SerializeField] private AnimationCurve _endFadeCurve;

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
        private void AssignStartPosition()
        {
            _startAnchoredPosition = _rectTransform.anchoredPosition;
        }

        [Button()]
        private void AssignStartScale()
        {
            _startScale = _rectTransform.localScale;
        }

        [Button()]
        private void AssignShowPosition()
        {
            _showAnchoredPosition = _rectTransform.anchoredPosition;
        }

        [Button()]
        private void AssignShowScale()
        {
            _showScale = _rectTransform.localScale;
        }

        [Button()]
        private void AssignEndPosition()
        {
            _endAnchoredPosition = _rectTransform.anchoredPosition;
        }

        [Button()]
        private void AssignEndScale()
        {
            _endScale = _rectTransform.localScale;
        }

        [Button()]
        public void Play()
        {
            Stop();

            _rectTransform.anchoredPosition = _startAnchoredPosition;
            _rectTransform.localScale = _startScale;
            _canvasGroup.alpha = _startAlpha;

            _sequence = DOTween.Sequence();
            _sequence
                .Append(CreateMoveTween(_showAnchoredPosition, _startAnchoredPositionCurve, _moveToShowDuration))
                .Join(CreateScaleTween(_showScale, _startScaleCurve, _moveToShowDuration))
                .Join(CreateFadeTween(_showAlpha, _startFadeCurve, _moveToShowDuration))
                .AppendInterval(_showDuration)
                .Append(CreateMoveTween(_endAnchoredPosition, _endAnchoredPositionCurve, _moveToEndDuration))
                .Join(CreateScaleTween(_endScale, _endScaleCurve, _moveToEndDuration))
                .Join(CreateFadeTween(_endAlpha, _endFadeCurve, _moveToEndDuration))
                .Play();
        }

        private Tween CreateMoveTween(Vector2 position, AnimationCurve curve, float duration)
        {
            return _rectTransform
                .DOAnchorPos(position, duration)
                .SetEase(curve);
        }

        private Tween CreateScaleTween(Vector3 scale, AnimationCurve curve, float duration)
        {
            return _rectTransform
                .DOScale(scale, duration)
                .SetEase(curve);
        }

        private Tween CreateFadeTween(float alpha, AnimationCurve curve, float duration)
        {
            return _canvasGroup
                .DOFade(alpha, duration)
                .SetEase(curve);
        }

        public void Stop()
        {
            _sequence?.Kill();
        }
    }
}
