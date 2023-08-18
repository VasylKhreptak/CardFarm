using CBA.Events.Core;
using NaughtyAttributes;
using UnityEngine;

namespace CBA.EventListeners.Core
{
    public abstract class EventListener : EventListenerCore
    {
        [Header("Listener Preferences")]
        [SerializeField] protected ListenerType _listenerType;

        [Header("Manual Listener Preferences")]
        [ShowIf(nameof(CanShowManualControl)), Required, SerializeField] private MonoEvent _addListenerEvent;
        [ShowIf(nameof(CanShowManualControl)), Required, SerializeField] private MonoEvent _removeListenerEvent;

        private bool CanShowManualControl() => _listenerType == ListenerType.FullyManual;

        #region MonoBehaviour

        protected virtual void OnValidate()
        {
            TryRunInEditor();
        }

        protected virtual void Awake()
        {
            if (_listenerType == ListenerType.AwakeDestroy)
            {
                TryAddListener();
            }
            else if (_listenerType == ListenerType.FullyManual)
            {
                _addListenerEvent.onMonoCall += TryAddListener;
                _removeListenerEvent.onMonoCall += TryRemoveListener;
            }
        }

        protected virtual void Start()
        {
            if (_listenerType == ListenerType.StartDestroy)
            {
                TryAddListener();
            }
        }

        protected virtual void OnEnable()
        {
            if (_listenerType == ListenerType.EnableDisable)
            {
                TryAddListener();
            }
        }

        protected virtual void OnDisable()
        {
            if (_listenerType == ListenerType.EnableDisable)
            {
                TryRemoveListener();
            }
        }

        protected virtual void OnDestroy()
        {
            if (_listenerType == ListenerType.AwakeDestroy ||
                _listenerType == ListenerType.StartDestroy)
            {
                TryRemoveListener();
            }
            else if (_listenerType == ListenerType.FullyManual)
            {
                _addListenerEvent.onMonoCall -= TryAddListener;
                _removeListenerEvent.onMonoCall -= TryRemoveListener;
            }
        }

        private void TryRunInEditor()
        {
            if (_listenerType == ListenerType.Editor)
            {
                TryAddListener();
            }
            else if (_listenerType != ListenerType.Editor)
            {
                TryRemoveListener();
            }
        }

        #endregion
    }
}
