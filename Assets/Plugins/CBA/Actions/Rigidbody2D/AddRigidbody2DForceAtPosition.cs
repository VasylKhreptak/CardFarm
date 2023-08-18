using CBA.Actions.Rigidbody2D.Core;
using UnityEngine;

namespace CBA.Actions.Rigidbody2D
{
    public class AddRigidbody2DForceAtPosition : Rigidbody2DAction
    {
        [Header("Preferences")]
        [SerializeField] private float _force;
        [SerializeField] private Vector2 _direction;
        [SerializeField] private Vector2 _relativeTo;
        [SerializeField] private ForceMode2D _forceMode = ForceMode2D.Force;

        public override void Do()
        {
            _rigidbody2D.AddForceAtPosition(_force * _direction, _relativeTo, _forceMode);
        }

        #region Editor

#if UNITY_EDITOR

        private void OnDrawGizmosSelected()
        {
            if (_rigidbody2D == null) return;

            DrawForceDirection();

            DrawRelativePoint();
        }

        private void DrawForceDirection()
        {
            Gizmos.color = Color.white;
            Extensions.Gizmos.DrawArrow(_rigidbody2D.position, _direction);
        }

        private void DrawRelativePoint()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_relativeTo, 0.1f);
        }

#endif

        #endregion

    }
}
