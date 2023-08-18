using CBA.Actions.Rigidbody.Core;
using UnityEngine;

namespace CBA.Actions.Rigidbody
{
    public class AddRigidbodyForceAtPosition : RigidbodyAction
    {
        [Header("Preferences")]
        [SerializeField] private float _force;
        [SerializeField] private Vector3 _direction;
        [SerializeField] private Vector3 _relativeTo;
        [SerializeField] private ForceMode _forceMode = ForceMode.Force;

        public override void Do()
        {
            _rigidbody.AddForceAtPosition(_force * _direction, _relativeTo, _forceMode);
        }

        #region Editor

#if UNITY_EDITOR

        private void OnDrawGizmosSelected()
        {
            if (_rigidbody == null) return;

            DrawForceDirection();

            DrawRelativePoint();
        }

        private void DrawForceDirection()
        {
            Gizmos.color = Color.white;
            Extensions.Gizmos.DrawArrow(_rigidbody.position, _direction);
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
