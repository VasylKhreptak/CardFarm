using CBA.Animations.Physics.Rigidbody2D.Move.Core;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace CBA.Animations.Physics.Rigidbody2D.Move
{
    public class Rigidbody2DMoveYAnimation : Rigidbody2DDirectionMoveAnimation
    {
        public override Tween CreateForwardAnimation()
        {
            return _rigidbody2D.DOMoveY(_to, _duration, _snapping);
        }

        public override Tween CreateBackwardAnimation()
        {
            return _rigidbody2D.DOMoveY(_from, _duration, _snapping);
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
            Vector2 position = _transform.position;
            Vector2 startPosition = new Vector2(position.x, _from);
            Vector2 targetPosition = new Vector2(position.x, _to);
            Vector2 direction = targetPosition - startPosition;

            Gizmos.color = Color.green;
            Extensions.Gizmos.DrawArrow(startPosition, direction);
        }

        private void DrawRecordingArrow()
        {
            Vector2 position = _transform.position;
            Vector2 startPosition = new Vector2(position.x, _from);
            Vector2 targetPosition = new Vector2(startPosition.x, position.y);
            Vector2 direction = targetPosition - startPosition;

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(startPosition, 0.1f);
            Extensions.Gizmos.DrawArrow(startPosition, direction);
        }

#endif

        #endregion

    }
}
