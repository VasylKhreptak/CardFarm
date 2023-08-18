using CBA.Animations.Transform.Move.Core;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace CBA.Animations.Transform.Move
{
    public class MoveYAnimation : DirectionMoveAnimation
    {
        public override Tween CreateForwardAnimation()
        {
            return _transform.DOMoveY(_to, _duration, _snapping);
        }

        public override Tween CreateBackwardAnimation()
        {
            return _transform.DOMoveY(_from, _duration, _snapping);
        }

        protected override void MoveTo(float axisPosition)
        {
            Vector3 position = _transform.position;
            _transform.position = new Vector3(position.x, axisPosition, position.z);
        }

        #region Editor

#if UNITY_EDITOR

        [ShowNonSerializedField] private bool _isRecording;

        [Button("Assign Start Position")]
        private void AssignStartPositionVariable()
        {
            if (_isRecording) return;

            _from = _transform.position.y;
        }

        [Button("Assign Target Position")]
        private void AssignTargetPosition()
        {
            if (_isRecording) return;

            _to = _transform.position.y;
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
            _from = _transform.position.y;

            _isRecording = true;
        }

        [Button("Stop Recording")]
        private void StopRecording()
        {
            if (_isRecording == false) return;

            _to = _transform.position.y;
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
            Vector3 position = _transform.position;
            Vector3 startPosition = new Vector3(position.x, _from, position.z);
            Vector3 targetPosition = new Vector3(position.x, _to, position.z);
            Vector3 direction = targetPosition - startPosition;

            Gizmos.color = Color.green;
            Extensions.Gizmos.DrawArrow(startPosition, direction);
        }

        private void DrawRecordingArrow()
        {
            Vector3 position = _transform.position;
            Vector3 startPosition = new Vector3(position.x, _from, position.z);
            Vector3 targetPosition = new Vector3(startPosition.x, position.y, startPosition.z);
            Vector3 direction = targetPosition - startPosition;

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(startPosition, 0.1f);
            Extensions.Gizmos.DrawArrow(startPosition, direction);
        }

#endif

        #endregion

    }
}
