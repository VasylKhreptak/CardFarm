using CBA.Animations.RectTransform.Core;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace CBA.Animations.RectTransform.Move.Pivot
{
    public class PivotMoveAnimation : RectTransformAnimation
    {
        [Header("Move Preferences")]
        [SerializeField] private Vector2 _startPivotPosition;
        [SerializeField] private Vector2 _targetPivotPosition;

        public override Tween CreateForwardAnimation()
        {
            return _rectTransform.DOPivot(_targetPivotPosition, _duration);
        }

        public override Tween CreateBackwardAnimation()
        {
            return _rectTransform.DOPivot(_startPivotPosition, _duration);
        }

        public override void MoveToStartState()
        {
            _rectTransform.pivot = _startPivotPosition;
        }

        public override void MoveToEndState()
        {
            _rectTransform.pivot = _targetPivotPosition;
        }

        #region Editor

#if UNITY_EDITOR

        [ShowNonSerializedField] private bool _isRecording;

        [Button("Assign Start Pivot")]
        private void AssignStartPivot()
        {
            if (_isRecording) return;

            _startPivotPosition = _rectTransform.pivot;
        }

        [Button("Assign Target Pivot")]
        private void AssignTargetPivot()
        {
            if (_isRecording) return;

            _targetPivotPosition = _rectTransform.pivot;
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
            _startPivotPosition = _rectTransform.pivot;

            _isRecording = true;
        }

        [Button("Stop Recording")]
        private void StopRecording()
        {
            if (_isRecording == false) return;

            _targetPivotPosition = _rectTransform.pivot;
            MoveToStartState();

            _isRecording = false;
        }

        private void OnDrawGizmosSelected()
        {
            if (_rectTransform == null) return;

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
            // Vector2 direction = _targetAnchoredPosition - _startAnchoredPosition;
            // Vector3 startPosition = _rectTransform.TransformPoint(_startAnchoredPosition);
            //
            // Gizmos.color = Color.white;
            // Extensions.Gizmos.DrawArrow(startPosition, direction);
        }

        private void DrawRecordingArrow()
        {
            // Vector2 anchoredPosition = _rectTransform.anchoredPosition;
            // Vector2 direction = anchoredPosition - _startAnchoredPosition;
            // Vector3 startPosition = _rectTransform.TransformPoint(_startAnchoredPosition);
            //
            // Gizmos.color = Color.red;
            // Gizmos.DrawSphere(_startAnchoredPosition, 0.1f);
            // Extensions.Gizmos.DrawArrow(startPosition, direction);
        }

#endif

        #endregion
        
    }
}
