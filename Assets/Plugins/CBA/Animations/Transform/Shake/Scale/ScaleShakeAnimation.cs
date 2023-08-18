using CBA.Animations.Transform.Shake.Core;
using DG.Tweening;
using UnityEngine;

namespace CBA.Animations.Transform.Shake.Scale
{
    public class ScaleShakeAnimation : ShakeAnimation
    {
        private Vector3 _startLocalScale;

        #region MonoBehaviour

        private void Awake()
        {
            _startLocalScale = _transform.localScale;
        }

        #endregion

        public override Tween CreateForwardAnimation()
        {
            return _transform.DOShakeScale(_duration, _strengthDirection * _strength, _vibrato, _randomness, _fadeOut, _shakeRandomnessMode);
        }

        public override Tween CreateBackwardAnimation()
        {
            return _transform.DOShakeScale(_duration, -_strengthDirection * _strength, _vibrato, _randomness, _fadeOut, _shakeRandomnessMode);

        }

        public override void MoveToStartState()
        {
            ResetLocalScale();
        }

        public override void MoveToEndState()
        {
            ResetLocalScale();
        }

        private void ResetLocalScale()
        {
            _transform.localScale = _startLocalScale;
        }

        #region Editor

#if UNITY_EDITOR

        protected override void DrawShake(UnityEngine.Transform parent)
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
