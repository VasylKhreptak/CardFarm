using CBA.Animations.Physics.Rigidbody.Move.Core;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace CBA.Animations.Physics.Rigidbody.Move
{
    public class RigidbodyMoveAnimation : Core.RigidbodyMoveAnimation
    {
        [Header("Move Preferences")]
        [SerializeField] private Vector3 _startPosition;
        [SerializeField] private Vector3 _targetPosition;

        public override Tween CreateForwardAnimation()
        {
            return _rigidbody.DOMove(_targetPosition, _duration, _snapping);
        }

        public override Tween CreateBackwardAnimation()
        {
            return _rigidbody.DOMove(_startPosition, _duration, _snapping);
        }

        public override void MoveToStartState()
        {
            _transform.position = _startPosition;
        }

        public override void MoveToEndState()
        {
            _transform.position = _targetPosition;
        }

        #region Editor

#if UNITY_EDITOR

        [ShowNonSerializedField] private bool _isRecording;

        [Button("Assign Start Position")]
        private void AssignStartPositionVariable()
        {
            if (_isRecording) return;

            _startPosition = _transform.position;
        }

        [Button("Assign Target Position")]
        private void AssignTargetPosition()
        {
            if (_isRecording) return;

            _targetPosition = _transform.position;
        }

        [Button("Move To Start")]
        private void MoveToStartPositionEditor()
        {
            if (_isRecording) return;

            MoveToStartState();
        }

        [Button("Move To End")]
        private void MoveToTargetPosition()
        {
            if (_isRecording) return;

            MoveToEndState();
        }

        [Button("Start Recording")]
        private void StartRecording()
        {
            _startPosition = _transform.position;

            _isRecording = true;
        }

        [Button("Stop Recording")]
        private void StopRecording()
        {
            if (_isRecording == false) return;

            _targetPosition = _transform.position;
            MoveToStartState();

            _isRecording = false;
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
                DrawDefaultArrow();
            }
        }

        private void DrawDefaultArrow()
        {
            Vector3 direction = _targetPosition - _startPosition;

            Gizmos.color = Color.white;
            Extensions.Gizmos.DrawArrow(_startPosition, direction);
        }

        private void DrawRecordingArrow()
        {
            Vector3 direction = _transform.position - _startPosition;

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_startPosition, 0.1f);
            Extensions.Gizmos.DrawArrow(_startPosition, direction);
        }

#endif

        #endregion

    }
}
