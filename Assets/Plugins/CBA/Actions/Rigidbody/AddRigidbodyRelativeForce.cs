﻿using CBA.Actions.Rigidbody.Core;
using UnityEngine;

namespace CBA.Actions.Rigidbody
{
    public class AddRigidbodyRelativeForce : RigidbodyAction
    {
        [Header("Preferences")]
        [SerializeField] private float _force;
        [SerializeField] private Vector3 _direction;
        [SerializeField] private ForceMode _forceMode = ForceMode.Force;

        public override void Do()
        {
            _rigidbody.AddRelativeForce(_force * _direction, _forceMode);
        }

        #region Editor

#if UNITY_EDITOR

        private void OnDrawGizmosSelected()
        {
            if (_rigidbody == null) return;

            DrawForceDirection();
        }

        private void DrawForceDirection()
        {
            Gizmos.color = Color.white;
            Extensions.Gizmos.DrawArrow(_rigidbody.position, _rigidbody.transform.TransformDirection(_direction));
        }

#endif

        #endregion

    }
}
