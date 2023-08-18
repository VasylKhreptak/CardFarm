using CBA.Animations.Audio.AudioSource.Core;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace CBA.Animations.Audio.AudioSource
{
    public class AudioSourcePitchAnimation : AudioSourceAnimation
    {
        [Header("Volume Preferences")]
        [SerializeField, Range(-3f, 3f)] private float _startPitch = 1f;
        [SerializeField, Range(-3f, 3f)] private float _targetPitch = 1f;

        public override Tween CreateForwardAnimation()
        {
            return _audioSource.DOPitch(_targetPitch, _duration);
        }

        public override Tween CreateBackwardAnimation()
        {
            return _audioSource.DOPitch(_startPitch, _duration);
        }

        public override void MoveToStartState()
        {
            _audioSource.pitch = _startPitch;
        }

        public override void MoveToEndState()
        {
            _audioSource.pitch = _targetPitch;
        }

        #region Editor

   #if UNITY_EDITOR

        [ShowNonSerializedField] private bool _isRecording;

        [Button("Assign Start Pitch")]
        private void AssignStartPitch()
        {
            if (_isRecording) return;

            _startPitch = _audioSource.pitch;
        }
        [Button("Assign Target Pitch")]
        private void AssignTargetPitch()
        {
            if (_isRecording) return;

            _targetPitch = _audioSource.pitch;
        }

        [Button("Set Start Pitch")]
        private void SetStartPitch()
        {
            if (_isRecording) return;

            MoveToStartState();
        }

        [Button("Set Target Pitch")]
        private void SetTargetPitch()
        {
            if (_isRecording) return;

            MoveToEndState();
        }

        [Button("Start Recording")]
        private void StartRecording()
        {
            _startPitch = _audioSource.pitch;

            _isRecording = true;
        }

        [Button("Stop Recording")]
        private void StopRecording()
        {
            if (_isRecording == false) return;

            _targetPitch = _audioSource.pitch;
            MoveToStartState();

            _isRecording = false;
        }

   #endif

        #endregion
    }
}
