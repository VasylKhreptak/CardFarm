using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace Graphics.Animations
{
    public class JumpFlipAnimation : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform _transform;

        [Header("Jump Preferences")]
        [SerializeField] private float _jumpHeight;
        [SerializeField] private Vector3 _jumpDirection;
        [SerializeField] private float _jumpDuration = 1f;
        [SerializeField] private AnimationCurve _jumpCurve;

        [Header("Flip Preferences")]
        [SerializeField] private int _flipCount = 3;
        [SerializeField] private Vector3 _flipAxis;

        [Header("Preferences")]
        [SerializeField] private float _jumpDelay = 0.5f;

        private Sequence _flipSequence;
        private Vector3 _initialLocalPosition;
        private Vector3 _targetLocalPosition;
        private Quaternion _initialLocalRotation;

        #region MonoBehaviour

        private void Awake()
        {
            _initialLocalPosition = _transform.localPosition;
            _targetLocalPosition = _initialLocalPosition + _jumpDirection * _jumpHeight;
            _initialLocalRotation = _transform.localRotation;
        }

        private void OnDisable()
        {
            _flipSequence.Kill();
            _transform.localPosition = _initialLocalPosition;
            _transform.localRotation = _initialLocalRotation;
        }

        private void OnValidate()
        {
            _transform ??= GetComponent<Transform>();
        }

        #endregion

        [Button()]
        public void StartFlipping()
        {
            _flipSequence?.OnStepComplete(() => {});

            if (_flipSequence != null && _flipSequence.IsActive()) return;
            
            Tween jumpTween = _transform.DOLocalMove(_targetLocalPosition, _jumpDuration)
                .SetEase(_jumpCurve)
                .SetLoops(2, LoopType.Yoyo);

            Tween rotateTween = _transform.DOLocalRotate(_flipAxis * 360f, _jumpDuration * 2 / _flipCount, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetLoops(_flipCount);


            _flipSequence = DOTween.Sequence()
                .Append(jumpTween)
                .Join(rotateTween)
                .AppendInterval(_jumpDelay)
                .SetLoops(-1)
                .Play();
        }

        [Button()]
        public void StopFlipping()
        {
            _flipSequence.OnStepComplete(() =>
            {
                _flipSequence.Kill();
                _transform.localPosition = _initialLocalPosition;
                _transform.localRotation = _initialLocalRotation;
            });
        }
    }
}
