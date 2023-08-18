using CBA.Animations.Graphics.Fade.Core;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace CBA.Animations.Graphics.Fade
{
    public class FadeAnimation : AlphaAnimation
    {
        [Header("Fade Preferences")]
        [SerializeField, Range(0f, 1f)] private float _startAlpha;
        [SerializeField, Range(0f, 1f)] private float _targetAlpha = 1f;

        public override Tween CreateForwardAnimation()
        {
            return DOTween.To(() => _alphaAdapter.alpha, x => _alphaAdapter.alpha = x, _targetAlpha, _duration);
        }

        public override Tween CreateBackwardAnimation()
        {
            return DOTween.To(() => _alphaAdapter.alpha, x => _alphaAdapter.alpha = x, _startAlpha, _duration);
        }

        public override void MoveToStartState()
        {
            _alphaAdapter.alpha = _startAlpha;
        }

        public override void MoveToEndState()
        {
            _alphaAdapter.alpha = _targetAlpha;
        }

        #region Editor

#if UNITY_EDITOR

        [ShowNonSerializedField] private bool _isRecording;

        [Button("Assign Start Alpha")]
        private void AssignStartAlpha()
        {
            if (_isRecording) return;

            _startAlpha = _alphaAdapter.alpha;
        }

        [Button("Assign Target Alpha")]
        private void AssignTargetAlpha()
        {
            if (_isRecording) return;

            _targetAlpha = _alphaAdapter.alpha;
        }

        [Button("Set Start Alpha")]
        private void SetStartAlpha()
        {
            if (_isRecording) return;

            MoveToStartState();
        }

        [Button("Set Target Alpha")]
        private void SetTargetAlpha()
        {
            if (_isRecording) return;

            MoveToEndState();
        }

        [Button("Start Recording")]
        private void StartRecording()
        {
            _startAlpha = _alphaAdapter.alpha;

            _isRecording = true;
        }

        [Button("Stop Recording")]
        private void StopRecording()
        {
            if (_isRecording == false) return;

            _targetAlpha = _alphaAdapter.alpha;
            MoveToStartState();

            _isRecording = false;
        }

#endif

        #endregion

    }
}
