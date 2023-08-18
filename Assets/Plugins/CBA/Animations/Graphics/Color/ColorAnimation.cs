using CBA.Adapters.Color.Core;
using CBA.Animations.Core;
using CBA.Animations.Graphics.Color.Core;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace CBA.Animations.Graphics.Color
{
    public class ColorAnimation : Core.ColorAnimation
    {
        [Header("Fade Preferences")]
        [SerializeField] private UnityEngine.Color _startColor = UnityEngine.Color.white;
        [SerializeField] private UnityEngine.Color _targetColor = UnityEngine.Color.white;

        public override Tween CreateForwardAnimation()
        {
            return DOTween.To(() => _colorAdapter.color, x => _colorAdapter.color = x, _targetColor, _duration);
        }

        public override Tween CreateBackwardAnimation()
        {
            return DOTween.To(() => _colorAdapter.color, x => _colorAdapter.color = x, _startColor, _duration);
        }

        public override void MoveToStartState()
        {
            _colorAdapter.color = _startColor;
        }

        public override void MoveToEndState()
        {
            _colorAdapter.color = _targetColor;
        }

        #region Editor

#if UNITY_EDITOR

        [ShowNonSerializedField] private bool _isRecording;

        [Button("Assign Start Alpha")]
        private void AssignStartAlpha()
        {
            if (_isRecording) return;

            _startColor = _colorAdapter.color;
        }

        [Button("Assign Target Alpha")]
        private void AssignTargetAlpha()
        {
            if (_isRecording) return;

            _targetColor = _colorAdapter.color;
        }

        [Button("Set Start Color")]
        private void SetStartColor()
        {
            if (_isRecording) return;

            MoveToStartState();
        }

        [Button("Set Target Color")]
        private void SetTargetColor()
        {
            if (_isRecording) return;

            MoveToEndState();
        }

        [Button("Start Recording")]
        private void StartRecording()
        {
            _startColor = _colorAdapter.color;

            _isRecording = true;
        }

        [Button("Stop Recording")]
        private void StopRecording()
        {
            if (_isRecording == false) return;

            _targetColor = _colorAdapter.color;
            MoveToStartState();

            _isRecording = false;
        }

#endif

        #endregion
    }
}
