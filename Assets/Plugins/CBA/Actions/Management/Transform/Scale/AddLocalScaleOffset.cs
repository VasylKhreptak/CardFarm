using CBA.Actions.Management.Transform.Core;
using NaughtyAttributes;
using UnityEngine;

namespace CBA.Actions.Management.Transform.Scale
{
    public class AddLocalScaleOffset : TransformAction
    {
        [Header("Preferences")]
        [SerializeField] private Vector3 _localScaleOffset;
        [SerializeField] private bool _smoothByDeltaTime;

        private Vector3 EvaluatedLocalScaleOffset => Extensions.Vector3.ReplaceWithByAxes(_localScaleOffset, Vector3.zero, Extensions.Vector3Int.InverseAxes(AllowedAxes));

        private Vector3 SmoothEvaluatedLocalScaleOffset => EvaluatedLocalScaleOffset * Time.deltaTime;

        public override void Do()
        {
            _transform.localScale += _smoothByDeltaTime ? SmoothEvaluatedLocalScaleOffset : EvaluatedLocalScaleOffset;
        }

        #region Editor

#if UNITY_EDITOR

        [ShowNonSerializedField] private Vector3 _startLocalScale;
        [ShowNonSerializedField] private bool _isRecording;
        [ShowNonSerializedField] private bool _movedToStart;
        [ShowNonSerializedField] private bool _movedToEnd;

        [HideIf(nameof(_smoothByDeltaTime)), Button("Start Recording")]
        private void StartRecording()
        {
            if (_isRecording) return;

            _startLocalScale = _transform.localScale;

            _isRecording = true;
            _movedToEnd = false;
            _movedToStart = false;
        }

        [HideIf(nameof(_smoothByDeltaTime)), Button("Stop Recording")]
        private void StopRecording()
        {
            if (_isRecording == false) return;

            _localScaleOffset = _transform.localScale - _startLocalScale;
            _transform.localScale = _startLocalScale;

            _isRecording = false;
            _movedToEnd = false;
            _movedToStart = false;
        }

        [HideIf(nameof(_smoothByDeltaTime)), Button("Scale To End")]
        private void ScaleToEnd()
        {
            if (_isRecording || _movedToEnd) return;

            _startLocalScale = _transform.localScale;
            _transform.localScale = _startLocalScale + EvaluatedLocalScaleOffset;

            _movedToEnd = true;
            _movedToStart = false;
        }

        [HideIf(nameof(_smoothByDeltaTime)), Button("Scale To Start")]
        private void ScaleToStart()
        {
            if (_isRecording || _movedToStart || _movedToEnd == false) return;

            _transform.localScale = _startLocalScale;

            _movedToStart = true;
            _movedToEnd = false;
        }

#endif

        #endregion

    }
}
