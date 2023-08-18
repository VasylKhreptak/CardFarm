using System;
using CBA.Animations.Audio.AudioMixer.Core;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace CBA.Animations.Audio.AudioMixer
{
    public class AudioMixerVolumeAnimation : AudioMixerAnimation
    {
        [Header("Volume Preferences")]
        [SerializeField, Range(-80f, 20f)] private float _startVolume = -80f;
        [SerializeField, Range(-80f, 20f)] private float _targetVolume;

        private float _volume
        {
            get
            {
                if (_audioMixerGroup.audioMixer.GetFloat(_audioMixerGroup.name, out float volume))
                {
                    return volume;
                }

                throw new Exception("There is no parameter of name: " + _audioMixerGroup.name);
            }
            set
            {
                if (_audioMixerGroup.audioMixer.SetFloat(_audioMixerGroup.name, value) == false)
                {
                    throw new Exception("There is no parameter of name: " + _audioMixerGroup.name);
                }
            }
        }

        public override Tween CreateForwardAnimation()
        {
            return _audioMixerGroup.audioMixer.DOSetFloat(_audioMixerGroup.name, _targetVolume, _duration);
        }

        public override Tween CreateBackwardAnimation()
        {
            return _audioMixerGroup.audioMixer.DOSetFloat(_audioMixerGroup.name, _startVolume, _duration);
        }

        public override void MoveToStartState()
        {
            _volume = _startVolume;
        }

        public override void MoveToEndState()
        {
            _volume = _targetVolume;
        }

        #region Editor

   #if UNITY_EDITOR

        [ShowNonSerializedField] private bool _isRecording;

        [Button("Assign Start Volume")]
        private void AssignStartVolume()
        {
            if (_isRecording) return;

            _startVolume = _volume;
        }
        [Button("Assign Target Volume")]
        private void AssignTargetVolume()
        {
            if (_isRecording) return;

            _targetVolume = _volume;
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
            _startVolume = _volume;

            _isRecording = true;
        }

        [Button("Stop Recording")]
        private void StopRecording()
        {
            if (_isRecording == false) return;

            _targetVolume = _volume;
            MoveToStartState();

            _isRecording = false;
        }

#endif

        #endregion

    }
}
