using CBA.Animations.Transform.Core;
using DG.Tweening;
using UnityEngine;

namespace CBA.Animations.Transform.Shake.Core
{
    public abstract class ShakeAnimation : TransformAnimation
    {
        [Header("Shake Preferences")]
        [SerializeField] protected Vector3 _strengthDirection;
        [SerializeField] protected float _strength = 5f;
        [SerializeField] protected int _vibrato = 10;
        [SerializeField] protected float _randomness = 90f;
        [SerializeField] protected bool _fadeOut = true;
        [SerializeField] protected ShakeRandomnessMode _shakeRandomnessMode = ShakeRandomnessMode.Full;

        #region Editor

#if UNITY_EDITOR

        private void OnDrawGizmosSelected()
        {
            UnityEngine.Transform parent = _transform.parent;

            if (_transform == null || parent == null) return;

            DrawShake(parent);
        }

        protected virtual void DrawShake(UnityEngine.Transform parent)
        {
            Vector3 position = parent.TransformPoint(_transform.localPosition);
            Vector3 direction = parent.TransformDirection(_strengthDirection * _strength);

            Gizmos.color = Color.white;
            Extensions.Gizmos.DrawArrow(position, direction);
        }

#endif

        #endregion
    }
}
