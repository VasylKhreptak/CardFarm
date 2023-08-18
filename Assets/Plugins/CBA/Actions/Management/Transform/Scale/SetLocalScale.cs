using CBA.Actions.Management.Transform.Core;
using NaughtyAttributes;
using UnityEngine;

namespace CBA.Actions.Management.Transform.Scale
{
    public class SetLocalScale : TransformAction
    {
        [Header("Preferences")]
        [SerializeField] private Vector3 _localScale;
        
        private Vector3 EvaluatedLocalScale => Extensions.Vector3.ReplaceWithByAxes(_transform.localScale, _localScale, AllowedAxes);
        
        public override void Do()
        {
            _transform.localScale = EvaluatedLocalScale;
        }
        
        #region Editor

#if UNITY_EDITOR
        
        [ShowNonSerializedField] private Vector3 _startLocalScale;
        [ShowNonSerializedField] private bool _isRecording;
        [ShowNonSerializedField] private bool _movedToStart;
        [ShowNonSerializedField] private bool _movedToEnd;

        [Button("Assign Local Scale")]
        private void AssignPosition()
        {
            _localScale = _transform.localScale;
        }
        
        [Button("Start Recording")]
        private void StartRecording()
        {
            if (_isRecording) return;

            _startLocalScale = _transform.localScale;

            _isRecording = true;
            _movedToEnd = false;
            _movedToStart = false;
        }

        [Button("Stop Recording")]
        private void StopRecording()
        {
            if (_isRecording == false) return;

            _localScale = _transform.localScale;
            _transform.localScale = _startLocalScale;

            _isRecording = false;
            _movedToEnd = false;
            _movedToStart = false;
        }

        [Button("Scale To End")]
        private void ScaleToEnd()
        {
            if (_isRecording || _movedToEnd) return;

            _startLocalScale = _transform.localScale;
            _transform.localScale = EvaluatedLocalScale;

            _movedToEnd = true;
            _movedToStart = false;
        }

        [Button("Scale To Start")]
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
