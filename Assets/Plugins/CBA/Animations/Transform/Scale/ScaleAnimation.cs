using CBA.Animations.Transform.Scale.Core;
using DG.Tweening;
using NaughtyAttributes;

namespace CBA.Animations.Transform.Scale
{
    public class ScaleAnimation : Core.ScaleAnimation
    {
        public override Tween CreateForwardAnimation()
        {
            return _transform.DOScale(_targetScale, _duration);
        }

        public override Tween CreateBackwardAnimation()
        {
            return _transform.DOScale(_startScale, _duration);
        }

        public override void MoveToStartState()
        {
            _transform.localScale = _startScale;
        }

        public override void MoveToEndState()
        {
            _transform.localScale = _targetScale;
        }

        #region Editor

#if UNITY_EDITOR

        [ShowNonSerializedField] private bool _isRecording;

        [Button("Assign Start Scale")]
        private void AssignStartScaleVariable()
        {
            if (_isRecording) return;

            _startScale = _transform.localScale;
        }

        [Button("Assign Target Scale")]
        private void AssignTargetScaleVariable()
        {
            if (_isRecording) return;

            _targetScale = _transform.localScale;
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
            _startScale = _transform.localScale;

            _isRecording = true;
        }

        [Button("Stop Recording")]
        private void StopRecording()
        {
            if (_isRecording == false) return;

            _targetScale = _transform.localScale;
            MoveToStartState();

            _isRecording = false;
        }

#endif

        #endregion

    }
}
