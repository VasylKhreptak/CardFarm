using CBA.Events.Core;
using UnityEngine;

namespace CBA.Events.Physics._3D
{
    public class OnTriggerExitMonoEvent : MonoEvent
    {
        [Header("References")]
        [SerializeField] private OnTriggerExitEvent _triggerExitEvent;

        #region MonoBehaviour

        private void OnValidate()
        {
            _triggerExitEvent ??= GetComponent<OnTriggerExitEvent>();
        }

        private void OnEnable()
        {
            _triggerExitEvent.onExit += Invoke;
        }

        private void OnDisable()
        {
            _triggerExitEvent.onExit -= Invoke;
        }

        #endregion

        private void Invoke(Collider collider) => Invoke();
    }
}
