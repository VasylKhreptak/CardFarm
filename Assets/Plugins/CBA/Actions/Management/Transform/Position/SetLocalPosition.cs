using CBA.Actions.Management.Transform.Core;
using NaughtyAttributes;
using UnityEngine;

namespace CBA.Actions.Management.Transform.Position
{
    public class SetLocalPosition : TransformAction
    {
        [Header("Preferences")]
        [SerializeField] private Vector3 _localPosition;

        private Vector3 EvaluatedLocalPosition => Extensions.Vector3.ReplaceWithByAxes(_transform.localPosition, _localPosition, AllowedAxes);
        
        public override void Do()
        {
            _transform.localPosition = EvaluatedLocalPosition;
        }

        #region Editor

#if UNITY_EDITOR

        [ShowNonSerializedField] private Vector3 _startLocalPosition;
        [ShowNonSerializedField] private bool _isRecording;
        [ShowNonSerializedField] private bool _movedToStart;
        [ShowNonSerializedField] private bool _movedToEnd;

        [Button("Assign Local Position")]
        private void AssignLocalPosition()
        {
            _localPosition = _transform.localPosition;
        }

        [Button("Start Recording")]
        private void StartRecording()
        {
            if (_isRecording) return;

            _startLocalPosition = _transform.localPosition;

            _isRecording = true;
            _movedToEnd = false;
            _movedToStart = false;
        }

        [Button("Stop Recording")]
        private void StopRecording()
        {
            if (_isRecording == false) return;

            _localPosition = _transform.localPosition;
            _transform.localPosition = _startLocalPosition;

            _isRecording = false;
            _movedToEnd = false;
            _movedToStart = false;
        }

        [Button("Move To End")]
        private void MoveToEnd()
        {
            if (_isRecording || _movedToEnd) return;

            _startLocalPosition = _transform.localPosition;
            _transform.localPosition = EvaluatedLocalPosition;

            _movedToEnd = true;
            _movedToStart = false;
        }

        [Button("Move To Start")]
        private void MoveToStart()
        {
            if (_isRecording || _movedToStart || _movedToEnd == false) return;

            _transform.localPosition = _startLocalPosition;

            _movedToStart = true;
            _movedToEnd = false;
        }

        private void OnDrawGizmosSelected()
        {
            UnityEngine.Transform parent = _transform.parent;

            if (_transform == null || parent == null) return;

            if (_isRecording)
            {
                DrawRecordingArrow(parent);
            }
            else
            {
                DrawArrow(parent);

                if (_movedToEnd)
                {
                    Vector3 pointPosition = parent.TransformPoint(_startLocalPosition);

                    DrawPoint(ref pointPosition);
                }
            }
        }

        private void DrawRecordingArrow(UnityEngine.Transform parent)
        {
            Vector3 transformLocalPosition = _transform.localPosition;
            Vector3 startPosition = parent.TransformPoint(_startLocalPosition);
            Vector3 targetLocalPosition = Extensions.Vector3.ReplaceWithByAxes(transformLocalPosition, _startLocalPosition, Extensions.Vector3Int.InverseAxes(AllowedAxes));
            Vector3 targetPosition = parent.TransformPoint(targetLocalPosition);
            Vector3 direction = targetPosition - startPosition;

            DrawRecordingArrow(ref startPosition, ref direction);
        }

        private void DrawArrow(UnityEngine.Transform parent)
        {
            Vector3 transformLocalPosition = _transform.localPosition;
            Vector3 startPosition = parent.TransformPoint(transformLocalPosition);
            Vector3 targetPosition = parent.TransformPoint(EvaluatedLocalPosition);
            Vector3 direction = targetPosition - startPosition;

            DrawArrow(ref startPosition, ref direction);
        }

        private void DrawArrow(ref Vector3 startPosition, ref Vector3 direction)
        {
            Gizmos.color = Color.white;
            Extensions.Gizmos.DrawArrow(startPosition, direction);
        }

        private void DrawRecordingArrow(ref Vector3 startPosition, ref Vector3 direction)
        {
            Gizmos.color = Color.red;
            DrawPoint(ref startPosition);
            Extensions.Gizmos.DrawArrow(startPosition, direction);
        }

        private void DrawPoint(ref Vector3 startPosition)
        {
            Gizmos.DrawSphere(startPosition, 0.1f);
        }
#endif

        #endregion
    }
}
