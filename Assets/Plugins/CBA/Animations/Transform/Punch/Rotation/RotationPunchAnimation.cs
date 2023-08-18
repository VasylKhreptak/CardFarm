using CBA.Animations.Transform.Punch.Core;
using DG.Tweening;
using UnityEngine;

namespace CBA.Animations.Transform.Punch.Rotation
{
    public class RotationPunchAnimation : PunchAnimation
    {
        private Quaternion _startLocalRotation;

        #region MonnBehaviour

        private void Awake()
        {
            _startLocalRotation = _transform.rotation;
        }

        #endregion

        public override Tween CreateForwardAnimation()
        {
            return _transform.DOPunchRotation(_strengthDirection * _strength, _duration, _vibrato, _elasticity);
        }

        public override Tween CreateBackwardAnimation()
        {
            return _transform.DOPunchRotation(-_strengthDirection * _strength, _duration, _vibrato, _elasticity);
        }

        public override void MoveToStartState()
        {
            ResetRotation();
        }

        public override void MoveToEndState()
        {
            ResetRotation();
        }

        private void ResetRotation()
        {
            _transform.localRotation = _startLocalRotation;
        }

        #region Editor

#if UNITY_EDITOR

        protected override void DrawPunch(UnityEngine.Transform parent)
        {
            Vector3 position = parent.TransformPoint(_transform.localPosition);
            Vector3 direction = _transform.TransformDirection(_strengthDirection * _strength);

            Gizmos.color = Color.white;
            Extensions.Gizmos.DrawArrow(position, direction);
        }

#endif
        
        #endregion
        
    }
}
