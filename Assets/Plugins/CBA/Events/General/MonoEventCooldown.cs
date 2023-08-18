using CBA.Delay;
using CBA.EventListeners;
using UnityEngine;

namespace CBA.Events.General
{
    public class MonoEventCooldown : MonoEventListener
    {
        [Header("Preferences")]
        [SerializeField] private float _cooldown;

        private bool _canInvoke = true;

        private Coroutine _waitCoroutine;

        protected override void OnEventFired()
        {
            TryInvoke();
        }

        private void TryInvoke()
        {
            if (_canInvoke)
            {
                Invoke();

                _canInvoke = false;
                _waitCoroutine = DelayManager.Wait(_cooldown, () => { _canInvoke = true; });
            }
        }

        public void StopCooldown()
        {
            DelayManager.StopWaiting(ref _waitCoroutine);
            _canInvoke = true;
        }
    }
}
