using CBA.Actions.Management.Transform.Core;
using NaughtyAttributes;
using UnityEngine;

namespace CBA.Actions.Management.Transform.Position
{
    public class AddLocalPositionOffset : TransformAction
    {
        [Header("Preferences")]
        [SerializeField] private Vector3 _localPositionOffset;
        [SerializeField] private bool _smoothByDeltaTime;

        private Vector3 EvaluatedLocalPositionOffset => Extensions.Vector3.ReplaceWithByAxes(_localPositionOffset, Vector3.zero, Extensions.Vector3Int.InverseAxes(AllowedAxes));

        private Vector3 SmoothEvaluatedLocalPositionOffset => EvaluatedLocalPositionOffset * Time.deltaTime;

        public override void Do()
        {
            _transform.localPosition += _smoothByDeltaTime ? SmoothEvaluatedLocalPositionOffset : EvaluatedLocalPositionOffset;
        }

        #region Editor

#if UNITY_EDITOR

        [ShowNonSerializedField] private bool _isRecording;
        [ShowNonSerializedField] private Vector3 _startLocalPosition;
        [ShowNonSerializedField] private bool _movedToStart;
        [ShowNonSerializedField] private bool _movedToEnd;

        [HideIf(nameof(_smoothByDeltaTime)), Button("Start Recording")]
        private void StartRecording()
        {
            if (_isRecording) return;

            _startLocalPosition = _transform.localPosition;

            _isRecording = true;
            _movedToEnd = false;
            _movedToStart = false;
        }

        [HideIf(nameof(_smoothByDeltaTime)), Button("Stop Recording")]
        private void StopRecording()
        {
            if (_isRecording == false) return;

            _localPositionOffset = _transform.localPosition - _startLocalPosition;
            _transform.localPosition = _startLocalPosition;

            _isRecording = false;
            _movedToEnd = false;
            _movedToStart = false;
        }

        [HideIf(nameof(_smoothByDeltaTime)), Button("Move To End")]
        private void MoveToEnd()
        {
            if (_isRecording || _movedToEnd) return;

            _startLocalPosition = _transform.localPosition;

            _transform.localPosition = _startLocalPosition + EvaluatedLocalPositionOffset;

            _movedToEnd = true;
            _movedToStart = false;
        }

        [HideIf(nameof(_smoothByDeltaTime)), Button("Move To Start")]
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
                OnRecording(parent);
            }
            else
            {
                DrawArrow(parent);

                if (_movedToEnd)
                {
                    Vector3 pointCenter = parent.TransformPoint(_startLocalPosition);

                    Gizmos.color = Color.white;
                    DrawPoint(ref pointCenter);
                }
            }
        }

        private void OnRecording(UnityEngine.Transform parent)
        {
            Vector3 transformLocalPosition = _transform.localPosition;
            Vector3 targetLocalPosition = Extensions.Vector3.ReplaceWithByAxes(transformLocalPosition, _startLocalPosition, Extensions.Vector3Int.InverseAxes(AllowedAxes));
            Vector3 startPosition = parent.TransformPoint(_startLocalPosition);
            Vector3 targetPosition = parent.TransformPoint(targetLocalPosition);
            Vector3 direction = targetPosition - startPosition;

            Gizmos.color = Color.red;
            DrawPoint(ref startPosition);
            Extensions.Gizmos.DrawArrow(startPosition, direction);
        }

        private void DrawArrow(UnityEngine.Transform parent)
        {
            Vector3 localPosition = _transform.localPosition;
            Vector3 startPosition = parent.TransformPoint(localPosition);
            Vector3 targetPosition = parent.TransformPoint(localPosition + EvaluatedLocalPositionOffset);
            Vector3 direction = targetPosition - startPosition;

            Gizmos.color = Color.white;
            Extensions.Gizmos.DrawArrow(startPosition, direction);
        }

        private void DrawPoint(ref Vector3 position)
        {
            Gizmos.DrawSphere(position, 0.1f);
        }

#endif

        #endregion
    }
}
