using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace UnlockedCardPanel.Graphics.Animations
{
    public class NewCardPanelShowAnimation : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private CanvasGroup _canvasGroup;

        [Header("General Preferences")]
        [SerializeField] private float _duration = 1f;

        [Header("Anchor Position Preferences")]
        [SerializeField] private Vector2 _startAnchorPos;
        [SerializeField] private Vector2 _endAnchorPos;
        [SerializeField] private AnimationCurve _anchorPosCurve;

        [Header("Scale Preferences")]
        [SerializeField] private Vector3 _startScale;
        [SerializeField] private Vector3 _endScale;
        [SerializeField] private AnimationCurve _scaleCurve;

        [Header("Fade Preferences")]
        [SerializeField] private float _startAlpha;
        [SerializeField] private float _endAlpha;
        [SerializeField] private AnimationCurve _fadeCurve;

        [Header("Size Delta Preferences")]
        [SerializeField] private Vector2 _startSizeDelta;
        [SerializeField] private Vector2 _endSizeDelta;
        [SerializeField] private AnimationCurve _sizeDeltaCurve;

        private Sequence _sequence;

        #region MonoBehaviour

        private void Awake()
        {
            SetStartValues();
        }

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
        public void Play()
        {
            Stop();

            _sequence = DOTween.Sequence();

            _sequence
                .Join(_rectTransform.DOScale(_endScale, _duration).SetEase(_scaleCurve))
                .Join(_rectTransform.DOSizeDelta(_endSizeDelta, _duration).SetEase(_sizeDeltaCurve))
                .Join(_canvasGroup.DOFade(_endAlpha, _duration).SetEase(_fadeCurve))
                .Join(_rectTransform.DOAnchorPos(_endAnchorPos, _duration).SetEase(_anchorPosCurve))
                .Play();
        }

        [Button()]
        public void Stop()
        {
            _sequence?.Kill();
        }

        private void SetStartValues()
        {
            _rectTransform.localScale = _startScale;
            _canvasGroup.alpha = _startAlpha;
            _rectTransform.sizeDelta = _startSizeDelta;
            _rectTransform.anchoredPosition = _startAnchorPos;
        }

        [Button()]
        private void AssignStartPos()
        {
            _startAnchorPos = _rectTransform.anchoredPosition;
        }

        [Button()]
        private void AssignEndPos()
        {
            _endAnchorPos = _rectTransform.anchoredPosition;
        }

        [Button()]
        private void AssignStartSizeDelta()
        {
            _startSizeDelta = _rectTransform.sizeDelta;
        }

        [Button()]
        private void AssignEndSizeDelta()
        {
            _endSizeDelta = _rectTransform.sizeDelta;
        }

        [Button()]
        private void AssignStartScale()
        {
            _startScale = _rectTransform.localScale;
        }

        [Button()]
        private void AssignEndScale()
        {
            _endScale = _rectTransform.localScale;
        }
    }
}
