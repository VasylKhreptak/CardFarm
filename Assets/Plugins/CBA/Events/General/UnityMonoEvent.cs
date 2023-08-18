using CBA.EventListeners;
using UnityEngine;
using UnityEngine.Events;

namespace CBA.Events.General
{
    public class UnityMonoEvent : MonoEventListener
    {
        [Header("Unity Event")]
        [SerializeField] private UnityEvent _unityEvent;

        protected override void OnEventFired()
        {
            _unityEvent.Invoke();
        }
    }
}
