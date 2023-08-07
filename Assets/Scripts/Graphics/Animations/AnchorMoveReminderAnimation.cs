using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace Graphics.Animations
{
    public class AnchorMoveReminderAnimation : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RectTransform _rectTransform;

        [Header("Preferences")]
        [SerializeField] private Vector2 _startPosition;
        [SerializeField] private Vector2 _endPosition;
        [SerializeField] private float _duration = 1f;
        [SerializeField] private float _delay = 1f;
        [SerializeField] private AnimationCurve _moveCurve;

        private Sequence _sequence;

        #region MonoBehaviour

        private void OnValidate()
        {
            _rectTransform ??= GetComponent<RectTransform>();
        }

        private void OnEnable()
        {
            StartAnimation();
        }

        private void OnDisable()
        {
            StopAnimation();
        }

        #endregion

        [Button()]
        private void AssignStartPosition()
        {
            _startPosition = _rectTransform.anchoredPosition;
        }
        
        [Button()]
        private void AssignEndPosition()
        {
            _endPosition = _rectTransform.anchoredPosition;
        }
        
        private void StartAnimation()
        {
            StopAnimation();

            _sequence = DOTween.Sequence();

            _sequence
                .AppendInterval(_delay)
                .Append(CreateMoveTween(_endPosition))
                .Append(CreateMoveTween(_startPosition))
                .SetLoops(-1)
                .Play();
        }

        private Tween CreateMoveTween(Vector2 targetPosition)
        {
            return _rectTransform.DOAnchorPos(targetPosition, _duration)
                .SetEase(_moveCurve);
        }

        private void StopAnimation()
        {
            _sequence?.Kill();
            _rectTransform.anchoredPosition = _startPosition;
        }
    }
}
