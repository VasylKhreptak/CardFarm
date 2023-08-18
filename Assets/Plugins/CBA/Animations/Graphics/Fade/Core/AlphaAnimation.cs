using CBA.Adapters.Alpha.Core;
using CBA.Animations.Core;
using NaughtyAttributes;
using UnityEngine;
using Animation = CBA.Animations.Core.Animation;

namespace CBA.Animations.Graphics.Fade.Core
{
    public abstract class AlphaAnimation : Animation
    {
        [Header("References")]
        [Required, SerializeField] protected AlphaAdapter _alphaAdapter;

        #region MonoBehaviour

        private void OnValidate()
        {
            _alphaAdapter ??= GetComponent<AlphaAdapter>();
        }

        #endregion
    }
}
