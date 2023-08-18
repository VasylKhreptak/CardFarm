using CBA.Animations.Core;
using NaughtyAttributes;
using UnityEngine;
using Animation = CBA.Animations.Core.Animation;

namespace CBA.Animations.Actions.Core
{
    public abstract class AnimationAction : CBA.Actions.Core.Action
    {
        [Header("References")]
        [Required, SerializeField] protected AnimationCore _animation;

        #region MonoBehaviour

        private void OnValidate()
        {
            _animation ??= GetComponent<AnimationCore>();
            
            if (transform.parent != null)
            {
                _animation ??= transform.parent.GetComponent<AnimationCore>();
            }
        }

        #endregion
    }
}
