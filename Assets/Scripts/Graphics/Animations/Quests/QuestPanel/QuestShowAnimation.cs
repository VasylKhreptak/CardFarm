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
        [SerializeField] private Vector2 _showAnchoredPosition;
        [SerializeField] private Vector2 _endAnchoredPosition;
        [SerializeField] private float _moveToShowDuration = 1f;
        [SerializeField] private float _showDuration;
        [SerializeField] private float _moveToEndDuration;
        [SerializeField] private AnimationCurve _moveToShowCurve;
        [SerializeField] private AnimationCurve _moveToEndCurve;

        [Header("Scale Preferences")]
        [SerializeField] private Vector3 _startScale;
        [SerializeField] private Vector3 _showScale;
        [SerializeField] private Vector3 _endScale;

        [Header("Fade Preferences")]
        [SerializeField] private float _startAlpha;
        [SerializeField] private float _showAlpha;
        [SerializeField] private float _endAlpha;

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
                .Append(CreateMoveTween(_showAnchoredPosition, _moveToShowCurve, _moveToShowDuration))
                .Join(CreateScaleTween(_showScale, _moveToShowCurve, _moveToShowDuration))
                .Join(CreateFadeTween(_showAlpha, _moveToShowCurve, _moveToShowDuration))
                .AppendInterval(_showDuration)
                .Append(CreateMoveTween(_endAnchoredPosition, _moveToEndCurve, _moveToEndDuration))
                .Join(CreateScaleTween(_endScale, _moveToEndCurve, _moveToEndDuration))
                .Join(CreateFadeTween(_endAlpha, _moveToEndCurve, _moveToEndDuration))
                .Play();
        }

        private Tween CreateMoveTween(Vector2 position, AnimationCurve curve, float duration)
        {
            return _rectTransform
                .DOAnchorPos(position, duration)
                .SetEase(curve);
        }

        private Tween CreateScaleTween(Vector2 scale, AnimationCurve curve, float duration)
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
