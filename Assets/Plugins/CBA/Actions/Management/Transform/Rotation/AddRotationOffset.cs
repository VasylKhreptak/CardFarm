using CBA.Actions.Management.Transform.Core;
using NaughtyAttributes;
using UnityEngine;

namespace CBA.Actions.Management.Transform.Rotation
{
    public class AddRotationOffset : TransformAction
    {
        [Header("Preferences")]
        [SerializeField] private Vector3 _rotationOffset;
        [SerializeField] private bool _smoothByDeltaTime;

        private Vector3 EvaluatedRotationOffset => Extensions.Vector3.ReplaceWithByAxes(_rotationOffset, Vector3.zero, Extensions.Vector3Int.InverseAxes(AllowedAxes));

        private Vector3 SmoothEvaluatedRotationOffset => EvaluatedRotationOffset * Time.deltaTime;

        public override void Do()
        {
            Quaternion rotation = _transform.rotation;

            _transform.rotation = Quaternion.Euler(_smoothByDeltaTime ? SmoothEvaluatedRotationOffset : EvaluatedRotationOffset) * rotation;
        }

        #region Editor

#if UNITY_EDITOR
        [ShowNonSerializedField] private Vector3 _startRotation;
        [ShowNonSerializedField] private bool _isRecording;
        [ShowNonSerializedField] private bool _rotatedToStart;
        [ShowNonSerializedField] private bool _rotatedToEnd;

        [HideIf(nameof(_smoothByDeltaTime)), Button("Start Recording")]
        private void StartRecording()
        {
            if (_isRecording) return;

            _startRotation = _transform.rotation.eulerAngles;

            _isRecording = true;
            _rotatedToEnd = false;
            _rotatedToStart = false;
        }

        [HideIf(nameof(_smoothByDeltaTime)), Button("Stop Recording")]
        private void StopRecording()
        {
            if (_isRecording == false) return;

            _rotationOffset = _transform.rotation.eulerAngles - _startRotation;
            _transform.rotation = Quaternion.Euler(_startRotation);

            _isRecording = false;
            _rotatedToEnd = false;
            _rotatedToStart = false;
        }

        [HideIf(nameof(_smoothByDeltaTime)), Button("Rotate To End")]
        private void RotateToEnd()
        {
            if (_isRecording || _rotatedToEnd) return;

            _startRotation = _transform.rotation.eulerAngles;

            Quaternion transformRotation = _transform.rotation;
            _transform.rotation = Quaternion.Euler(EvaluatedRotationOffset) * transformRotation;

            _rotatedToEnd = true;
            _rotatedToStart = false;
        }

        [HideIf(nameof(_smoothByDeltaTime)), Button("Rotate To Start")]
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
