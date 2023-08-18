using CBA.Animations.Transform.Core;
using UnityEngine;

namespace CBA.Animations.Transform.Punch.Core
{
    public abstract class PunchAnimation : TransformAnimation
    {
        [Header("Punch Preferences")]
        [SerializeField] protected Vector3 _strengthDirection;
        [SerializeField] protected float _strength = 5f;
        [SerializeField] protected int _vibrato = 10;
        [SerializeField] protected float _elasticity = 1f;

        #region Editor

#if UNITY_EDITOR

        private void OnDrawGizmosSelected()
        {
            UnityEngine.Transform parent = _transform.parent;

            if (_transform == null || parent == null) return;

            DrawPunch(parent);
        }

        protected virtual void DrawPunch(UnityEngine.Transform parent)
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
