using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace Graphics.Animations.Quests.RewardPanel
{
    public class RewardPanelShowAnimation : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private RectTransform _iconRectTransform;
        [SerializeField] private CanvasGroup _iconCanvasGroup;

        [Header("Preferences")]
        [SerializeField] private float _generalDuration = 1f;

        [Header("Fade Preferences")]
        [SerializeField] private float _startAlpha;
        [SerializeField] private float _endAlpha;
        [SerializeField] private AnimationCurve _fadeCurve;

        [Header("Scale Preferences")]
        [SerializeField] private Vector3 _startScale;
        [SerializeField] private Vector3 _endScale;
        [SerializeField] private AnimationCurve _scaleCurve;

        [Header("Size Delta Preferences")]
        [SerializeField] private float _sizeDeltaDelay;
        [SerializeField] private Vector2 _startSizeDelta;
        [SerializeField] private Vector2 _endSizeDelta;
        [SerializeField] private AnimationCurve _sizeDeltaCurve;
        [SerializeField] private float _sizeDeltaDuration;

        [Header("Icon Animation Preferences")]
        [SerializeField] private float _iconDelay;
        [SerializeField] private float _iconDuration;

        [Header("Icon Fade Preferences")]
        [SerializeField] private float _startIconAlpha;
        [SerializeField] private float _endIconAlpha;
        [SerializeField] private AnimationCurve _iconFadeCurve;

        [Header("Icon Scale Preferences")]
        [SerializeField] private Vector3 _startIconScale;
        [SerializeField] private Vector3 _endIconScale;
        [SerializeField] private AnimationCurve _iconScaleCurve;

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
        public void Play()
        {
            Stop();

            _canvasGroup.alpha = _startAlpha;
            _rectTransform.localScale = _startScale;
            _rectTransform.sizeDelta = _startSizeDelta;

            _iconRectTransform.localScale = _startIconScale;
            _iconCanvasGroup.alpha = _startIconAlpha;

            Sequence iconSequence = DOTween.Sequence();

            iconSequence
                .AppendInterval(_iconDelay)
                .Append(_iconCanvasGroup.DOFade(_endIconAlpha, _iconDuration).SetEase(_iconFadeCurve))
                .Join(_iconRectTransform.DOScale(_endIconScale, _iconDuration).SetEase(_iconScaleCurve));

            Sequence mainSequence = DOTween.Sequence();

            mainSequence
                .Join(_rectTransform.DOScale(_endScale, _generalDuration).SetEase(_scaleCurve))
                .Join(_canvasGroup.DOFade(_endAlpha, _generalDuration).SetEase(_fadeCurve))
                .AppendInterval(_sizeDeltaDelay)
                .Append(_rectTransform.DOSizeDelta(_endSizeDelta, _sizeDeltaDuration).SetEase(_sizeDeltaCurve));

            _sequence = DOTween.Sequence();

            _sequence
                .Join(iconSequence)
                .Join(mainSequence)
                .Play();
        }

        public void Stop()
        {
            _sequence?.Kill();
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
    }
}
