using CBA.Actions.Management.Transform.Core;
using NaughtyAttributes;
using UnityEngine;

namespace CBA.Actions.Management.Transform.Position
{
    public class AddPositionOffset : TransformAction
    {
        [Header("Preferences")]
        [SerializeField] private Vector3 _offset;
        [SerializeField] private bool _smoothByDeltaTime;

        private Vector3 EvaluatedOffset => Extensions.Vector3.ReplaceWithByAxes(_offset, Vector3.zero, Extensions.Vector3Int.InverseAxes(AllowedAxes));

        private Vector3 SmoothEvaluatedOffset => EvaluatedOffset * Time.deltaTime;

        public override void Do()
        {
            _transform.position += _smoothByDeltaTime ? SmoothEvaluatedOffset : EvaluatedOffset;
        }

        #region Editor

#if UNITY_EDITOR

        [ShowNonSerializedField] private Vector3 _startPosition;
        [ShowNonSerializedField] private bool _isRecording;
        [ShowNonSerializedField] private bool _movedToStart;
        [ShowNonSerializedField] private bool _movedToEnd;

        [HideIf(nameof(_smoothByDeltaTime)), Button("Start Recording")]
        private void StartRecording()
        {
            if (_isRecording) return;

            _startPosition = _transform.position;

            _isRecording = true;
            _movedToEnd = false;
            _movedToStart = false;
        }

        [HideIf(nameof(_smoothByDeltaTime)), Button("Stop Recording")]
        private void StopRecording()
        {
            if (_isRecording == false) return;

            _offset = _transform.position - _startPosition;
            _transform.position = _startPosition;

            _isRecording = false;
            _movedToEnd = false;
            _movedToStart = false;
        }

        [HideIf(nameof(_smoothByDeltaTime)), Button("Move To End")]
        private void MoveToEnd()
        {
            if (_isRecording || _movedToEnd) return;

            _startPosition = _transform.position;

            _transform.position = _startPosition + EvaluatedOffset;

            _movedToEnd = true;
            _movedToStart = false;
        }

        [HideIf(nameof(_smoothByDeltaTime)), Button("Move To Start")]
        private void MoveToStart()
        {
            if (_isRecording || _movedToStart || _movedToEnd == false) return;

            _transform.position = _startPosition;

            _movedToStart = true;
            _movedToEnd = false;
        }

        private void OnDrawGizmosSelected()
        {
            if (_transform == null) return;

            if (_isRecording)
            {
                DrawRecordingArrow();
            }
            else
            {
                DrawArrow();

                if (_movedToEnd)
                {
                    Gizmos.color = Color.white;
                    DrawPoint(ref _startPosition);
                }
            }
        }

        private void DrawRecordingArrow()
        {
            Vector3 targetPosition = Extensions.Vector3.ReplaceWithByAxes(_transform.position, _startPosition, Extensions.Vector3Int.InverseAxes(AllowedAxes));
            Vector3 direction = targetPosition - _startPosition;

            Gizmos.color = Color.red;
            DrawPoint(ref _startPosition);
            Extensions.Gizmos.DrawArrow(_startPosition, direction);
        }

        private void DrawArrow()
        {
            Vector3 startPosition = _transform.position;
            Vector3 targetPosition = startPosition + EvaluatedOffset;
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
