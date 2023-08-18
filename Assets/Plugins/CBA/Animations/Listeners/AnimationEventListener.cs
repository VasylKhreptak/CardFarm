using CBA.Animations.Core;
using CBA.EventListeners.Core;
using NaughtyAttributes;
using UnityEngine;
using Animation = CBA.Animations.Core.Animation;

namespace CBA.Animations.Listeners
{
    public abstract class AnimationEventListener : EventListenerCore
    {
        [Header("References")]
        [Required, SerializeField] protected  AnimationCore _animation;

        [Header("Listener Preferences")]
        [SerializeField] private AnimationListenerType _listenerType = AnimationListenerType.OnEnableOnDisable;

        private bool _addedInitListener;
        
        #region MonoBehaviour

        private void OnValidate()
        {
            _animation ??= GetComponent<Animation>();
        }

        private void Awake()
        {
            if (_listenerType == AnimationListenerType.AwakeOnDestroy)
            {
                TryAddInitListener();
            }
        }

        private void Start()
        {
            if (_listenerType == AnimationListenerType.StartOnDestroy)
            {
                TryAddInitListener();
            }
        }

        private void OnEnable()
        {
            if (_listenerType == AnimationListenerType.OnEnableOnDisable)
            {
                TryAddInitListener();
            }
        }

        private void OnDisable()
        {
            if (_listenerType == AnimationListenerType.OnEnableOnDisable)
            {
                TryRemoveAllListeners();
            }
        }

        private void OnDestroy()
        {
            if (_listenerType == AnimationListenerType.AwakeOnDestroy || 
                _listenerType == AnimationListenerType.StartOnDestroy)
            {
                TryRemoveAllListeners();
            }
        }

        #endregion

        private void TryAddInitListener()
        {
            if (_addedInitListener == false)
            {
                _animation.onInit += OnAnimationInit;

                _addedInitListener = true;
            }
        }

        private void TryRemoveInitListener()
        {
            if (_addedInitListener)
            {
                _animation.onInit -= OnAnimationInit;

                _addedInitListener = false;
            }
        }
        
        private void OnAnimationInit()
        {
            TryRemoveListener();
            
            TryAddListener();
        }

        private void TryRemoveAllListeners()
        {
            TryRemoveInitListener();
            
            TryRemoveListener();
        }
    }
}
