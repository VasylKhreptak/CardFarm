using CBA.Actions.Core;
using CBA.Animations.Core;
using NaughtyAttributes;
using UnityEngine;

namespace CBA.Animations.Actions.Core
{
    public abstract class AnimationsAction : Action
    {
        [Header("References")]
        [Required, SerializeField] protected AnimationCore[] _animations;

        #region MonoBehaviour

        private void OnValidate()
        {
            if (_animations == null || _animations.Length == 0)
            {
                _animations = GetComponents<AnimationCore>();
            }
        }

        #endregion
    }
}
