using CBA.Animations.RectTransform.Core;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace CBA.Animations.RectTransform.Move.Pivot
{
    public class PivotMoveXAnimation : RectTransformAnimation
    {
        [Header("Move Preferences")]
        [SerializeField] private float _startPivotXPosition;
        [SerializeField] private float _targetPivotXPosition;

        public override Tween CreateForwardAnimation()
        {
            return _rectTransform.DOPivotX(_startPivotXPosition, _duration);
        }

        public override Tween CreateBackwardAnimation()
        {
            return _rectTransform.DOPivotX(_targetPivotXPosition, _duration);
        }

        public override void MoveToStartState()
        {
            Vector2 pivot = _rectTransform.pivot;
            _rectTransform.pivot = new Vector2(_startPivotXPosition, pivot.y);
        }

        public override void MoveToEndState()
        {
            Vector2 pivot = _rectTransform.pivot;
            _rectTransform.pivot = new Vector2(_targetPivotXPosition, pivot.y);
        }

        #region Editor

#if UNITY_EDITOR

        [ShowNonSerializedField] private bool _isRecording;

        [Button("Assign Start Pivot")]
        private void AssignStartPivot()
        {
            if (_isRecording) return;

            _startPivotXPosition = _rectTransform.pivot.x;
        }

        [Button("Assign Target Pivot")]
        private void AssignTargetPivot()
        {
            if (_isRecording) return;

            _targetPivotXPosition = _rectTransform.pivot.x;
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
            _startPivotXPosition = _rectTransform.pivot.x;

            _isRecording = true;
        }

        [Button("Stop Recording")]
        private void StopRecording()
        {
            if (_isRecording == false) return;

            _targetPivotXPosition = _rectTransform.pivot.x;
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
