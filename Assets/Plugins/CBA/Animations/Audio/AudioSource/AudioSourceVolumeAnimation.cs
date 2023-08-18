using CBA.Animations.Audio.AudioSource.Core;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace CBA.Animations.Audio.AudioSource
{
    public class AudioSourceVolumeAnimation : AudioSourceAnimation
    {
        [Header("Volume Preferences")]
        [SerializeField, Range(0f, 1f)] private float _startVolume;
        [SerializeField, Range(0f, 1f)] private float _targetVolume = 1f;

        public override Tween CreateForwardAnimation()
        {
            return _audioSource.DOFade(_targetVolume, _duration);
        }

        public override Tween CreateBackwardAnimation()
        {
            return _audioSource.DOFade(_startVolume, _duration);
        }

        public override void MoveToStartState()
        {
            _audioSource.volume = _startVolume;
        }

        public override void MoveToEndState()
        {
            _audioSource.volume = _targetVolume;
        }

        #region Editor

#if UNITY_EDITOR

        [ShowNonSerializedField] private bool _isRecording;

        [Button("Assign Start Volume")]
        private void AssignStartVolume()
        {
            if (_isRecording) return;

            _startVolume = _audioSource.volume;
        }
        [Button("Assign Target Volume")]
        private void AssignTargetVolume()
        {
            if (_isRecording) return;

            _targetVolume = _audioSource.volume;
        }

        [Button("Set Start Volume")]
        private void SetStartVolume()
        {
            if (_isRecording) return;

            MoveToStartState();
        }

        [Button("Set Target Volume")]
        private void SetTargetVolume()
        {
            if (_isRecording) return;

            MoveToEndState();
        }

        [Button("Start Recording")]
        private void StartRecording()
        {
            _startVolume = _audioSource.volume;

            _isRecording = true;
        }

        [Button("Stop Recording")]
        private void StopRecording()
        {
            if (_isRecording == false) return;

            _targetVolume = _audioSource.volume;
            MoveToStartState();

            _isRecording = false;
        }

#endif

        #endregion

    }
}
