using CBA.Actions.Rigidbody.Core;
using UnityEngine;

namespace CBA.Actions.Rigidbody
{
    public class AddRigidbodyExplosionForce : RigidbodyAction
    {
        [Header("Preferences")]
        [SerializeField] private float _force;
        [SerializeField] private Vector3 _explodePosition;
        [SerializeField] private float _radius = 1;
        [SerializeField] private float _upwardsModifier;
        [SerializeField] private ForceMode _forceMode = ForceMode.Force;

        public override void Do()
        {
            _rigidbody.AddExplosionForce(_force, _explodePosition, _radius, _upwardsModifier, _forceMode);
        }

        #region Editor

#if UNITY_EDITOR

        private void OnDrawGizmosSelected()
        {
            if (_rigidbody == null) return;

            Gizmos.color = Color.red;

            DrawRadius();

            DrawExplosionPosition();
        }

        private void DrawRadius()
        {
            Gizmos.DrawWireSphere(_explodePosition, _radius);
        }

        private void DrawExplosionPosition()
        {
            Gizmos.DrawSphere(_explodePosition, 0.1f);
        }

#endif

        #endregion

    }
}
