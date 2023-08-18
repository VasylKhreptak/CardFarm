using CBA.Actions.Management.Transform.Core;
using NaughtyAttributes;
using UnityEngine;

namespace CBA.Actions.Management.Transform.Rotation
{
    public class AddLocalRotationOffset : TransformAction
    {
        [Header("Preferences")]
        [SerializeField] private Vector3 _localRotationOffset;
        [SerializeField] private bool _smoothByDeltaTime;

        private Vector3 EvaluatedLocalRotationOffset => Extensions.Vector3.ReplaceWithByAxes(_localRotationOffset, Vector3.zero, Extensions.Vector3Int.InverseAxes(AllowedAxes));

        private Vector3 SmoothEvaluatedLocalRotationOffset => EvaluatedLocalRotationOffset * Time.deltaTime;

        public override void Do()
        {
            Quaternion transformLocalRotation = _transform.localRotation;
            _transform.localRotation = Quaternion.Euler(_smoothByDeltaTime ? SmoothEvaluatedLocalRotationOffset : EvaluatedLocalRotationOffset) * transformLocalRotation;
        }

        #region Editor

#if UNITY_EDITOR
        [ShowNonSerializedField] private Vector3 _startLocalRotation;
        [ShowNonSerializedField] private bool _isRecording;
        [ShowNonSerializedField] private bool _rotatedToStart;
        [ShowNonSerializedField] private bool _rotatedToEnd;

        [HideIf(nameof(_smoothByDeltaTime)), Button("Start Recording")]
        private void StartRecording()
        {
            if (_isRecording) return;

            _startLocalRotation = _transform.localRotation.eulerAngles;

            _isRecording = true;
            _rotatedToEnd = false;
            _rotatedToStart = false;
        }

        [HideIf(nameof(_smoothByDeltaTime)), Button("Stop Recording")]
        private void StopRecording()
        {
            if (_isRecording == false) return;

            _localRotationOffset = _transform.localRotation.eulerAngles - _startLocalRotation;
            _transform.localRotation = Quaternion.Euler(_startLocalRotation);

            _isRecording = false;
            _rotatedToEnd = false;
            _rotatedToStart = false;
        }

        [HideIf(nameof(_smoothByDeltaTime)), Button("Rotate To End")]
        private void RotateToEnd()
        {
            if (_isRecording || _rotatedToEnd) return;

            _startLocalRotation = _transform.localRotation.eulerAngles;

            Quaternion transformLocalRotation = _transform.localRotation;

            _transform.localRotation = Quaternion.Euler(EvaluatedLocalRotationOffset) * transformLocalRotation;

            _rotatedToEnd = true;
            _rotatedToStart = false;
        }

        [HideIf(nameof(_smoothByDeltaTime)), Button("Rotate To Start")]
        private void RotateToStart()
        {
            if (_isRecording || _rotatedToStart || _rotatedToEnd == false) return;

            _transform.localRotation = Quaternion.Euler(_startLocalRotation);

            _rotatedToStart = true;
            _rotatedToEnd = false;
        }

#endif

        #endregion

    }
}
