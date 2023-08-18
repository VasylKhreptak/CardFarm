using CBA.Actions.Management.Transform.Core;
using NaughtyAttributes;
using UnityEngine;

namespace CBA.Actions.Management.Transform.Rotation
{
    public class SetRotation : TransformAction
    {
        [Header("Preferences")]
        [SerializeField] private Vector3 _rotation;

        private Vector3 EvaluatedRotation => Extensions.Vector3.ReplaceWithByAxes(_transform.rotation.eulerAngles, _rotation, AllowedAxes);

        public override void Do()
        {
            _transform.rotation = Quaternion.Euler(EvaluatedRotation);
        }

        #region Editor

#if UNITY_EDITOR
        [ShowNonSerializedField] private Vector3 _startRotation;
        [ShowNonSerializedField] private bool _isRecording;
        [ShowNonSerializedField] private bool _rotatedToStart;
        [ShowNonSerializedField] private bool _rotatedToEnd;

        [Button("Assign Rotation")]
        private void AssignRotationVariable()
        {
            if (_isRecording) return;

            _rotation = _transform.rotation.eulerAngles;
        }

        [Button("Start Recording")]
        private void StartRecording()
        {
            if (_isRecording) return;

            _startRotation = _transform.rotation.eulerAngles;

            _isRecording = true;
            _rotatedToEnd = false;
            _rotatedToStart = false;
        }

        [Button("Stop Recording")]
        private void StopRecording()
        {
            if (_isRecording == false) return;

            _rotation = _transform.rotation.eulerAngles;
            _transform.rotation = Quaternion.Euler(_startRotation);

            _isRecording = false;
            _rotatedToEnd = false;
            _rotatedToStart = false;
        }

        [Button("Rotate To End")]
        private void RotateToEnd()
        {
            if (_isRecording || _rotatedToEnd) return;

            _startRotation = _transform.rotation.eulerAngles;

            _transform.rotation = Quaternion.Euler(EvaluatedRotation);

            _rotatedToEnd = true;
            _rotatedToStart = false;
        }

        [Button("Rotate To Start")]
        private void RotateToStart()
        {
            if (_isRecording || _rotatedToStart || _rotatedToEnd == false) return;

            _transform.rotation = Quaternion.Euler(_startRotation);

            _rotatedToStart = true;
            _rotatedToEnd = false;
        }

#endif

        #endregion

    }
}
