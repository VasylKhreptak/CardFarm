using CBA.Actions.Rigidbody.Core;
using UnityEngine;

namespace CBA.Actions.Rigidbody
{
    public class SetRigidbodyVelocity : RigidbodyAction
    {
        [Header("Preferences")]
        [SerializeField] private float _velocity;
        [SerializeField] private Vector3 _direction;

        public override void Do()
        {
            _rigidbody.velocity = _velocity * _direction;
        }

        #region Editor

#if UNITY_EDITOR

        private void OnDrawGizmosSelected()
        {
            if (_rigidbody == null) return;

            DrawDirection();
        }

        private void DrawDirection()
        {
            Gizmos.color = Color.white;
            Extensions.Gizmos.DrawArrow(_rigidbody.position, _direction);
        }

#endif

        #endregion

    }
}
