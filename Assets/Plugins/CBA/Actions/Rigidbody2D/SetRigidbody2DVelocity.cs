using CBA.Actions.Rigidbody2D.Core;
using UnityEngine;

namespace CBA.Actions.Rigidbody2D
{
    public class SetRigidbody2DVelocity : Rigidbody2DAction
    {
        [Header("Preferences")]
        [SerializeField] private float _velocity;
        [SerializeField] private Vector2 _direction;

        public override void Do()
        {
            _rigidbody2D.velocity = _velocity * _direction;
        }

        #region Editor

#if UNITY_EDITOR

        private void OnDrawGizmosSelected()
        {
            if (_rigidbody2D == null) return;

            DrawDirection();
        }

        private void DrawDirection()
        {
            Gizmos.color = Color.white;
            Extensions.Gizmos.DrawArrow(_rigidbody2D.position, _direction);
        }

#endif

        #endregion

    }
}
