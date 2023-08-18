using CBA.Animations.Physics.Rigidbody.Core;
using CBA.Animations.Transform.Move.PositionProvider.Core;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace CBA.Animations.Physics.Rigidbody.Look
{
    public class RigidbodyLookAtAnimation : RigidbodyAnimation
    {
        [Header("Look At Preferences")]
        [Required, SerializeField] protected PositionProvider _starTarget;
        [Required, SerializeField] protected PositionProvider _endTarget;
        [SerializeField] protected AxisConstraint _axisConstraint = AxisConstraint.None;
        [SerializeField] protected Vector3 _up = Vector3.up;

        public override Tween CreateForwardAnimation()
        {
            return _rigidbody.DOLookAt(_endTarget.position, _duration, _axisConstraint, _up);
        }

        public override Tween CreateBackwardAnimation()
        {
            return _rigidbody.DOLookAt(_starTarget.position, _duration, _axisConstraint, _up);
        }

        public override void MoveToStartState()
        {
            Vector3 lookDirection = _starTarget.position - _transform.position;
            Vector3 lookRotation = Quaternion.LookRotation(lookDirection, _up).eulerAngles;
            Vector3Int replaceAxes = Extensions.Vector3Int.InverseAxes(Extensions.AxisConstraint.ToVector3Int(_axisConstraint));
            Vector3 evaluatedRotation = Extensions.Vector3.ReplaceWithByAxes(_transform.rotation.eulerAngles, lookRotation, replaceAxes);
            _transform.rotation = Quaternion.Euler(evaluatedRotation);
        }

        public override void MoveToEndState()
        {
            Vector3 lookDirection = _endTarget.position - _transform.position;
            Vector3 lookRotation = Quaternion.LookRotation(lookDirection, _up).eulerAngles;
            Vector3Int replaceAxes = Extensions.Vector3Int.InverseAxes(Extensions.AxisConstraint.ToVector3Int(_axisConstraint));
            Vector3 evaluatedRotation = Extensions.Vector3.ReplaceWithByAxes(_transform.rotation.eulerAngles, lookRotation, replaceAxes);
            _transform.rotation = Quaternion.Euler(evaluatedRotation);
        }

        #region Editor

#if UNITY_EDITOR

        [Button("Look At Start")]
        private void LookAtStart()
        {
            MoveToStartState();
        }

        [Button("Look At Target")]
        private void LookAtTarget()
        {
            MoveToEndState();
        }

        private void OnDrawGizmosSelected()
        {
            if (_transform == null || _starTarget == null || _endTarget == null) return;

            DrawTargets();
        }

        private void DrawTargets()
        {
            DrawStartTarget();

            DrawEndTarget();
        }

        private void DrawStartTarget()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_starTarget.position, 0.1f);
            Gizmos.DrawLine(_transform.position, _starTarget.position);
        }

        private void DrawEndTarget()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(_endTarget.position, 0.1f);
            Gizmos.DrawLine(_transform.position, _endTarget.position);
        }

#endif

        #endregion

    }
}
