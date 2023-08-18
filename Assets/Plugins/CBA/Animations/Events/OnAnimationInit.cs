using CBA.Animations.Core;
using CBA.EventListeners.Core;
using NaughtyAttributes;
using UnityEngine;

namespace CBA.Animations.Events
{
    public class OnAnimationInit : EventListener
    {
        [Header("References")]
        [Required, SerializeField] private AnimationCore _animation;
        
        protected override void AddListener()
        {
            _animation.onInit += Invoke;
        }
        protected override void RemoveListener()
        {
            _animation.onInit -= Invoke;
        }
    }
}
