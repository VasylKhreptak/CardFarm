using CBA.Actions.Management.Transform.Core;
using NaughtyAttributes;
using UnityEngine;

namespace CBA.Actions.Management.Transform.Rotation
{
    public class SetLocalRotation : TransformAction
    {
        [Header("Preferences")]
        [SerializeField] private Vector3 _localRotation;

        Vector3 EvaluatedLocalRotation => Extensions.Vector3.ReplaceWithByAxes(_transform.localRotation.eulerAngles, _localRotation, AllowedAxes);
        
        public override void Do()
        {
            _transform.localRotation = Quaternion.Euler(EvaluatedLocalRotation);
        }

        #region Editor

#if UNITY_EDITOR
        [ShowNonSerializedField] private Vector3 _startRotation;
        [ShowNonSerializedField] private bool _isRecording;
        [ShowNonSerializedField] private bool _rotatedToStart;
        [ShowNonSerializedField] private bool _rotatedToEnd;

        [Button("Assign Local Rotation")]
        private void AssignLocalRotationVariable()
        {
            if (_isRecording) return;

            _localRotation = _transform.localRotation.eulerAngles;
        }

        [Button("Start Recording")]
        private void StartRecording()
        {
            if (_isRecording) return;

            _startRotation = _transform.localRotation.eulerAngles;

            _isRecording = true;
            _rotatedToEnd = false;
            _rotatedToStart = false;
        }

        [Button("Stop Recording")]
        private void StopRecording()
        {
            if (_isRecording == false) return;

            _localRotation = _transform.localRotation.eulerAngles;
            _transform.localRotation = Quaternion.Euler(_startRotation);

            _isRecording = false;
            _rotatedToEnd = false;
            _rotatedToStart = false;
        }

        [Button("Rotate To End")]
        private void RotateToEnd()
        {
            if (_isRecording || _rotatedToEnd) return;

            _startRotation = _transform.localRotation.eulerAngles;

            _transform.localRotation = Quaternion.Euler(EvaluatedLocalRotation);

            _rotatedToEnd = true;
            _rotatedToStart = false;
        }

        [Button("Rotate To Start")]
        private void RotateToStart()
        {
            if (_isRecording || _rotatedToStart || _rotatedToEnd == false) return;

            _transform.localRotation = Quaternion.Euler(_startRotation);

            _rotatedToStart = true;
            _rotatedToEnd = false;
        }

#endif

        #endregion

    }
}
