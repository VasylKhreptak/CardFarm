using CBA.Delay;
using CBA.EventListeners;
using UnityEngine;

namespace CBA.Events.General
{
    public class DelayedMonoEvent : MonoEventListener
    {
        [Header("Wait Preferences")]
        [SerializeField] private float _delay = 2f;

        private Coroutine _waitRoutine;

        protected override void OnEventFired()
        {
            DelayManager.StopWaiting(ref _waitRoutine);
            _waitRoutine = DelayManager.Wait(_delay, Invoke);
        }

        public void StopDelay()
        {
            DelayManager.StopWaiting(ref _waitRoutine);
        }
    }
}
