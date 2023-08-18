using System.Collections.Generic;
using System.Linq;
using CBA.EventListeners.Core;
using CBA.Events.Core;
using UnityEngine;

namespace CBA.Events.General
{
    public class MonoEventGroup : EventListener
    {
        [Header("Events")]
        [SerializeField] private MonoEvent[] _events;

        #region MonoBehaviour

        protected override void OnValidate()
        {
            base.OnValidate();
            
            if (_events == null)
            {
                _events = transform.GetComponents<MonoEvent>();
                List<MonoEvent> _eventsList = _events.ToList();
                _eventsList.Remove(this);
                _events = _eventsList.ToArray();
            }
        }

        #endregion
        
        protected override void AddListener()
        {
            foreach (var @event in _events)
            {
                @event.onMonoCall += Invoke;
            }
        }
        protected override void RemoveListener()
        {
            foreach (var @event in _events)
            {
                @event.onMonoCall -= Invoke;
            }
        }
    }
}
