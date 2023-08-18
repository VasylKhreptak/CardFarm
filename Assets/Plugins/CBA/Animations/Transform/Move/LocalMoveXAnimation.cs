using CBA.Animations.Transform.Move.Core;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace CBA.Animations.Transform.Move
{
    public class LocalMoveXAnimation : DirectionMoveAnimation
    {
        public override Tween CreateForwardAnimation()
        {
            return _transform.DOLocalMoveX(_to, _duration, _snapping);
        }

        public override Tween CreateBackwardAnimation()
        {
            return _transform.DOLocalMoveX(_from, _duration, _snapping);
        }

        protected override void MoveTo(float axisPosition)
        {
            Vector3 localPosition = _transform.localPosition;
            _transform.localPosition = new Vector3(axisPosition, localPosition.y, localPosition.z);
        }

        #region Editor

#if UNITY_EDITOR

        [ShowNonSerializedField] private bool _isRecording;

        [Button("Assign Start Position")]
        private void AssignStartPositionVariable()
        {
            if (_isRecording) return;

            _from = _transform.localPosition.x;
        }

        [Button("Assign Target Position")]
        private void AssignTargetPosition()
        {
            if (_isRecording) return;

            _to = _transform.localPosition.x;
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
            _from = _transform.localPosition.x;

            _isRecording = true;
        }

        [Button("Stop Recording")]
        private void StopRecording()
        {
            if (_isRecording == false) return;

            _to = _transform.localPosition.x;
            MoveToStartState();

            _isRecording = false;
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
                DrawDefaultArrow(parent);
            }
        }

        private void DrawDefaultArrow(UnityEngine.Transform parent)
        {
            Vector3 localPosition = _transform.localPosition;
            Vector3 startLocalPosition = new Vector3(_from, localPosition.y, localPosition.z);
            Vector3 targetLocalPosition = new Vector3(_to, localPosition.y, localPosition.z);
            Vector3 startPosition = parent.TransformPoint(startLocalPosition);
            Vector3 targetPosition = parent.TransformPoint(targetLocalPosition);
            Vector3 direction = targetPosition - startPosition;

            Gizmos.color = Color.red;
            Extensions.Gizmos.DrawArrow(startPosition, direction);
        }

        private void DrawRecordingArrow(UnityEngine.Transform parent)
        {
            Vector3 localPosition = _transform.localPosition;
            Vector3 startLocalPosition = new Vector3(_from, localPosition.y, localPosition.z);
            Vector3 targetLocalPosition = new Vector3(localPosition.x, startLocalPosition.y, startLocalPosition.z);
            Vector3 startPosition = parent.TransformPoint(startLocalPosition);
            Vector3 targetPosition = parent.TransformPoint(targetLocalPosition);
            Vector3 direction = targetPosition - startPosition;

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(startPosition, 0.1f);
            Extensions.Gizmos.DrawArrow(startPosition, direction);
        }

#endif

        #endregion

    }
}
