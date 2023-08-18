using CBA.Events.Core;
using UnityEngine;

namespace CBA.Events.Physics._2D
{
    public class OnTriggerExit2DMonoEvent : MonoEvent
    {
        [Header("References")]
        [SerializeField] private OnTriggerExit2DEvent _triggerExitEvent;

        #region MonoBehaviour

        private void OnValidate()
        {
            _triggerExitEvent ??= GetComponent<OnTriggerExit2DEvent>();
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

        private void Invoke(Collider2D collider) => Invoke();
    }
}
