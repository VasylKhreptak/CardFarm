using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace Graphics.Animations
{
    public class FadeHighlighter : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CanvasGroup _canvasGroup;

        [Header("Preferences")]
        [SerializeField] private float _duration;
        [SerializeField] private float _minAlpha = 0.3f;
        [SerializeField] private float _maxAlpha = 1f;
        [SerializeField] private AnimationCurve _curve;

        private Sequence _sequence;

        #region MonoBehaviour

        private void OnValidate()
        {
            _canvasGroup ??= GetComponent<CanvasGroup>();
        }

        private void OnDisable()
        {
            StopHighlighting();
        }

        #endregion

        [Button()]
        public void StartHighlighting()
        {
            _canvasGroup.gameObject.SetActive(true);

            StopHighlighting();
            SetAlpha(_minAlpha);

            _sequence = DOTween.Sequence();

            _sequence
                .Append(_canvasGroup.DOFade(_maxAlpha, _duration).SetEase(_curve))
                .SetLoops(-1, LoopType.Yoyo)
                .Play();
        }

        [Button()]
        public void StopHighlighting()
        {
            KillSequence();

            _canvasGroup.gameObject.SetActive(false);
        }

        private void SetAlpha(float alpha) => _canvasGroup.alpha = alpha;

        private void KillSequence() => _sequence?.Kill();
    }
}
