using CBA.Actions.Core;
using CBA.Events.General;
using UnityEngine;

namespace CBA.Actions.Events
{
    public class StopDelayedEvent : Action
    {
        [Header("References")]
        [SerializeField] private DelayedMonoEvent _delayedMonoEvent;

        public override void Do()
        {
            _delayedMonoEvent.StopDelay();
        }
    }
}
