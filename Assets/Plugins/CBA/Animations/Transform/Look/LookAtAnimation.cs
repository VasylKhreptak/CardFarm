using CBA.Animations.Transform.Look.Core;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using AxisConstraint = CBA.Extensions.AxisConstraint;

namespace CBA.Animations.Transform.Look
{
    public class LookAtAnimation : Core.LookAtAnimation
    {
        public override Tween CreateForwardAnimation()
        {
            return _transform.DOLookAt(_endTarget.position, _duration, _axisConstraint, _up);
        }

        public override Tween CreateBackwardAnimation()
        {
            return _transform.DOLookAt(_starTarget.position, _duration, _axisConstraint, _up);
        }

        public override void MoveToStartState()
        {
            Vector3 lookDirection = _starTarget.position - _transform.position;
            Vector3 lookRotation = Quaternion.LookRotation(lookDirection, _up).eulerAngles;
            Vector3Int replaceAxes = Extensions.Vector3Int.InverseAxes(AxisConstraint.ToVector3Int(_axisConstraint));
            Vector3 evaluatedRotation = Extensions.Vector3.ReplaceWithByAxes(_transform.rotation.eulerAngles, lookRotation, replaceAxes);
            _transform.rotation = Quaternion.Euler(evaluatedRotation);
        }

        public override void MoveToEndState()
        {
            Vector3 lookDirection = _endTarget.position - _transform.position;
            Vector3 lookRotation = Quaternion.LookRotation(lookDirection, _up).eulerAngles;
            Vector3Int replaceAxes = Extensions.Vector3Int.InverseAxes(AxisConstraint.ToVector3Int(_axisConstraint));
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
