using CBA.Animations.Transform.Scale.Core;
using DG.Tweening;
using NaughtyAttributes;
using Vector3 = UnityEngine.Vector3;

namespace CBA.Animations.Transform.Scale
{
    public class ScaleXAnimation : DirectionScaleAnimation
    {
        public override Tween CreateForwardAnimation()
        {
            return _transform.DOScaleX(_to, _duration);
        }

        public override Tween CreateBackwardAnimation()
        {
            return _transform.DOScaleX(_from, _duration);
        }

        public override void MoveToStartState()
        {
            Vector3 scale = _transform.localScale;
            _transform.localScale = new Vector3(_from, scale.y, scale.z);
        }

        public override void MoveToEndState()
        {
            Vector3 scale = _transform.localScale;
            _transform.localScale = new Vector3(_to, scale.y, scale.z);
        }

        #region Editor

#if UNITY_EDITOR

        [ShowNonSerializedField] private bool _isRecording;

        [Button("Assign Start Scale")]
        private void AssignStartScaleVariable()
        {
            if (_isRecording) return;

            _from = _transform.localScale.x;
        }

        [Button("Assign Target Scale")]
        private void AssignTargetScaleVariable()
        {
            if (_isRecording) return;

            _to = _transform.localScale.x;
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
            _from = _transform.localScale.x;

            _isRecording = true;
        }

        [Button("Stop Recording")]
        private void StopRecording()
        {
            if (_isRecording == false) return;

            _to = _transform.localScale.x;
            MoveToStartState();

            _isRecording = false;
        }

#endif

        #endregion

    }
}
