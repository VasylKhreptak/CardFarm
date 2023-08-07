using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace Graphics.Animations.Reminder
{
    public class AnchorPositionPunchReminderAnimation : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RectTransform _rectTransform;

        [Header("Preferences")]
        [SerializeField] private float _startDelay = 1f;
        [SerializeField] private float _repeatInterval = 1f;
        [SerializeField] private Vector2 _force;
        [SerializeField] private float _duration;
        [SerializeField] private int vibrato = 10;
        [SerializeField] private float _elasticity = 1f;
        [SerializeField] private AnimationCurve _curve = AnimationCurve.Linear(0, 0, 1, 1);

        private Vector2 _initialAnchoredPosition;

        private Sequence _sequence;

        #region MonoBehaviour

        private void OnValidate()
        {
            _rectTransform ??= GetComponent<RectTransform>();
        }

        private void Awake()
        {
            _initialAnchoredPosition = _rectTransform.anchoredPosition;
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
                .SetDelay(_startDelay)
                .AppendCallback(() => _rectTransform.anchoredPosition = _initialAnchoredPosition)
                .Append(_rectTransform.DOPunchAnchorPos(_force, _duration, vibrato, _elasticity))
                .AppendInterval(_repeatInterval)
                .SetLoops(-1, LoopType.Restart)
                .SetEase(_curve)
                .Play();
        }

        [Button()]
        public void Stop()
        {
            _sequence?.Kill();
        }
    }
}
