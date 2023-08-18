using CBA.Events.Core;
using UnityEngine;

namespace CBA.Events.Physics._2D
{
    public class OnTriggerStay2DMonoEvent : MonoEvent
    {
        [Header("References")]
        [SerializeField] private OnTriggerStay2DEvent _triggerStayEvent;

        #region MonoBehaviour

        private void OnValidate()
        {
            _triggerStayEvent ??= GetComponent<OnTriggerStay2DEvent>();
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

        private void Invoke(Collider2D collider) => Invoke();
    }
}
