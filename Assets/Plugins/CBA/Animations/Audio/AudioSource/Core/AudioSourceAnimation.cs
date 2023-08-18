using CBA.Animations.Core;
using NaughtyAttributes;
using UnityEngine;
using Animation = CBA.Animations.Core.Animation;

namespace CBA.Animations.Audio.AudioSource.Core
{
    public abstract class AudioSourceAnimation : Animation
    {
        [Header("References")]
        [Required, SerializeField] protected UnityEngine.AudioSource _audioSource;

        #region MonoBehaviour

        private void OnValidate()
        {
            _audioSource ??= GetComponent<UnityEngine.AudioSource>();
        }

        #endregion
    }
}
