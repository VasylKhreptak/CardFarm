using CBA.Animations.Transform.Punch.Core;
using DG.Tweening;
using UnityEngine;

namespace CBA.Animations.Transform.Punch.Move
{
    public class PositionPunchAnimation : PunchAnimation
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
            return _transform.DOPunchPosition(_strengthDirection * _strength, _duration, _vibrato, _elasticity, _snapping);
        }

        public override Tween CreateBackwardAnimation()
        {
            return _transform.DOPunchPosition(-_strengthDirection * _strength, _duration, _vibrato, _elasticity, _snapping);
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
