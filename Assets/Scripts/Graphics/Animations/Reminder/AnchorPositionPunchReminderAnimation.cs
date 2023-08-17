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
        [SerializeField] private Vector2 _initialAnchoredPosition;
        [SerializeField] private float _startDelay = 1f;
        [SerializeField] private float _repeatInterval = 1f;
        [SerializeField] private Vector2 _force;
        [SerializeField] private float _duration;
        [SerializeField] private int vibrato = 10;
        [SerializeField] private float _elasticity = 1f;
        [SerializeField] private AnimationCurve _curve = AnimationCurve.Linear(0, 0, 1, 1);
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
            SetStartPosition();
        }

        #endregion

        [Button()]
        public void Play()
        {
            Stop();

            SetStartPosition();

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
        public void Stop(bool setStartPositionSmoothly = false)
        {
            _sequence?.Kill();
        }

        private void SetStartPosition()
        {
            _rectTransform.anchoredPosition = _initialAnchoredPosition;
        }

        public void SetStartPositionSmoothly()
        {
            Stop();

            _sequence = DOTween.Sequence();
            _sequence
                .Append(_rectTransform.DOAnchorPos(_initialAnchoredPosition, _duration).SetEase(_curve))
                .Play();
        }

        [Button()]
        private void AssignInitialPosition()
        {
            _initialAnchoredPosition = _rectTransform.anchoredPosition;
        }
    }
}
