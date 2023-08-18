using CBA.Animations.Transform.Scale.Core;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace CBA.Animations.Transform.Scale
{
    public class ScaleYAnimation : DirectionScaleAnimation
    {
        public override Tween CreateForwardAnimation()
        {
            return _transform.DOScaleY(_to, _duration);
        }

        public override Tween CreateBackwardAnimation()
        {
            return _transform.DOScaleY(_from, _duration);
        }

        public override void MoveToStartState()
        {
            Vector3 scale = _transform.localScale;
            _transform.localScale = new Vector3(scale.x, _from, scale.z);
        }

        public override void MoveToEndState()
        {
            Vector3 scale = _transform.localScale;
            _transform.localScale = new Vector3(scale.x, _to, scale.z);
        }

        #region Editor

#if UNITY_EDITOR

        [ShowNonSerializedField] private bool _isRecording;

        [Button("Assign Start Scale")]
        private void AssignStartScaleVariable()
        {
            if (_isRecording) return;

            _from = _transform.localScale.y;
        }

        [Button("Assign Target Scale")]
        private void AssignTargetScaleVariable()
        {
            if (_isRecording) return;

            _to = _transform.localScale.y;
        }

        [Button("Move To Start")]
        private void MoveToStartScale()
        {
            if (_isRecording) return;

            MoveToStartState();
        }

        [Button("Move To End")]
        private void MoveToTargetScale()
        {
            if (_isRecording) return;

            MoveToEndState();
        }

        [Button("Start Recording")]
        private void StartRecording()
        {
            _from = _transform.localScale.y;

            _isRecording = true;
        }

        [Button("Stop Recording")]
        private void StopRecording()
        {
            if (_isRecording == false) return;

            _to = _transform.localScale.y;
            MoveToStartState();

            _isRecording = false;
        }

#endif

        #endregion
        
    }
}
