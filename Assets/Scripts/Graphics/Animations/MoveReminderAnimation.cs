using DG.Tweening;
using UnityEngine;

namespace Graphics.Animations
{
    public class MoveReminderAnimation : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform _transform;

        [Header("Preferences")]
        [SerializeField] private Vector3 _startPosition;
        [SerializeField] private Vector3 _endPosition;
        [SerializeField] private float _duration = 1f;
        [SerializeField] private float _delay = 1f;
        [SerializeField] private AnimationCurve _moveCurve;

        private Sequence _sequence;

        #region MonoBehaviour

        private void OnValidate()
        {
            _transform ??= GetComponent<Transform>();
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

        private Tween CreateMoveTween(Vector3 targetPosition)
        {
            return _transform
                .DOMove(targetPosition, _duration)
                .SetEase(_moveCurve);
        }

        private void StopAnimation()
        {
            _sequence?.Kill();
            _transform.position = _startPosition;
        }
    }
}
