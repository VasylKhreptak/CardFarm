using CBA.Animations.Core;
using UnityEngine;
using UnityEngine.Audio;
using Animation = CBA.Animations.Core.Animation;

namespace CBA.Animations.Audio.AudioMixer.Core
{
    public abstract class AudioMixerAnimation : Animation
    {
        [Header("References")]
        [SerializeField] protected AudioMixerGroup _audioMixerGroup;
    }
}
