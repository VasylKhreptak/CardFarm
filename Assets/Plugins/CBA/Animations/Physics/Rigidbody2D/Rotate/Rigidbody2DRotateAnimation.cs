using CBA.Animations.Physics.Rigidbody2D.Core;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace CBA.Animations.Physics.Rigidbody2D.Rotate
{
    public class Rigidbody2DRotateAnimation : Rigidbody2DAnimation
    {
        [Header("Rotate Preferences")]
        [SerializeField] private float _startAngle;
        [SerializeField] private float _targetAngle;

        public override Tween CreateForwardAnimation()
        {
            return _rigidbody2D.DORotate(_targetAngle, _duration);
        }

        public override Tween CreateBackwardAnimation()
        {
            return _rigidbody2D.DORotate(_startAngle, _duration);
        }

        public override void MoveToStartState()
        {
            Vector3 rotation = _transform.rotation.eulerAngles;
            _transform.rotation = Quaternion.Euler(rotation.x, rotation.y, _startAngle);
        }

        public override void MoveToEndState()
        {
            Vector3 rotation = _transform.rotation.eulerAngles;
            _transform.rotation = Quaternion.Euler(rotation.x, rotation.y, _targetAngle);
        }

        #region Editor

#if UNITY_EDITOR

        [ShowNonSerializedField] private bool _isRecording;

        [Button("Assign Start Angle")]
        private void AssignStartAngle()
        {
            if (_isRecording) return;

            _startAngle = _transform.rotation.eulerAngles.z;
        }

        [Button("Assign Target Position")]
        private void AssignTargetAngle()
        {
            if (_isRecording) return;

            _targetAngle = _transform.rotation.eulerAngles.z;
        }

        [Button("Rotate To Start")]
        private void RotateToStart()
        {
            if (_isRecording) return;

            MoveToStartState();
        }

        [Button("Rotate To End")]
        private void RotateToEnd()
        {
            if (_isRecording) return;

            MoveToEndState();
        }

        [Button("Start Recording")]
        private void StartRecording()
        {
            _startAngle = _transform.rotation.eulerAngles.z;

            _isRecording = true;
        }

        [Button("Stop Recording")]
        private void StopRecording()
        {
            if (_isRecording == false) return;

            _targetAngle = _transform.rotation.eulerAngles.z;
            MoveToStartState();

            _isRecording = false;
        }

        private void OnDrawGizmosSelected()
        {
            if (_transform == null) return;

            DrawAngle();
        }

        private void DrawAngle()
        {
            Vector2 startDirection = Quaternion.Euler(new Vector3(0, 0, _startAngle)) * Vector2.up;
            Vector2 targetDirection = Quaternion.Euler(new Vector3(0, 0, _targetAngle)) * Vector2.up;

            Color arcColor = Extensions.Color.WithAlpha(Color.blue, 0.3f);
            Extensions.Gizmos.DrawAngle(_transform.position, startDirection, targetDirection, 2f, arcColor, Color.white);
        }

#endif

        #endregion

    }
}
