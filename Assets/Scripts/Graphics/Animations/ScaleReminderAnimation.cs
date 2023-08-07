using DG.Tweening;
using UnityEngine;

namespace Graphics.Animations
{
    public class ScaleReminderAnimation : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform _transform;

        [Header("Preferences")]
        [SerializeField] private Vector3 _startScale;
        [SerializeField] private Vector3 _endScale;
        [SerializeField] private float _duration = 1f;
        [SerializeField] private AnimationCurve _scaleCurve;
        [SerializeField] private float _delay = 1f;

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

            // _sequence = DOTween.Sequence();
            //
            // _sequence
            //     .AppendInterval(_delay)
            //     .Append(CreateScaleTween(_endScale))
            //     .Append(CreateScaleTween(_startScale))
            //     .SetLoops(-1)
            //     .Play();
        }

        private Tween CreateScaleTween(Vector3 targetScale)
        {
            return _transform
                .DOScale(targetScale, _duration)
                .SetEase(_scaleCurve);
        }

        private void StopAnimation()
        {
            _sequence?.Kill();
            _transform.localScale = _startScale;
        }
    }
}
