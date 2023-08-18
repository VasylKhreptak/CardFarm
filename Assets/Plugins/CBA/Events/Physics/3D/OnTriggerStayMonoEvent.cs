using CBA.Events.Core;
using UnityEngine;

namespace CBA.Events.Physics._3D
{
    public class OnTriggerStayMonoEvent : MonoEvent
    {
        [Header("References")]
        [SerializeField] private OnTriggerStayEvent _triggerStayEvent;

        #region MonoBehaviour

        private void OnValidate()
        {
            _triggerStayEvent ??= GetComponent<OnTriggerStayEvent>();
        }

        private void OnEnable()
        {
            _triggerStayEvent.onStay += Invoke;
        }

        private void OnDisable()
        {
            _triggerStayEvent.onStay -= Invoke;
        }

        #endregion

        private void Invoke(Collider collider) => Invoke();
    }
}
