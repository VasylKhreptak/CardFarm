using CBA.Animations.Transform.Shake.Core;
using DG.Tweening;
using UnityEngine;

namespace CBA.Animations.Transform.Shake.Position
{
    public class PositionShakeAnimation : ShakeAnimation
    {
        [Header("Snapping")]
        [SerializeField] private bool _snapping;

        private Vector3 _startLocalPosition;

        #region MonoBehaviour

        private void Awake()
        {
            _startLocalPosition = _transform.localPosition;
        }

        #endregion

        public override Tween CreateForwardAnimation()
        {
            return _transform.DOShakePosition(_duration, _strengthDirection * _strength, _vibrato, _randomness, _snapping, _fadeOut, _shakeRandomnessMode);
        }

        public override Tween CreateBackwardAnimation()
        {
            return _transform.DOShakePosition(_duration, -_strengthDirection * _strength, _vibrato, _randomness, _snapping, _fadeOut, _shakeRandomnessMode);

        }

        public override void MoveToStartState()
        {
            ResetLocalPosition();
        }

        public override void MoveToEndState()
        {
            ResetLocalPosition();
        }

        private void ResetLocalPosition()
        {
            _transform.localPosition = _startLocalPosition;
        }
    }
}
