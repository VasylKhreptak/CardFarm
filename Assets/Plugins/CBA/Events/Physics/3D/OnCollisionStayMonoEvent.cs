using CBA.Events.Core;
using UnityEngine;

namespace CBA.Events.Physics._3D
{
    public class OnCollisionStayMonoEvent : MonoEvent
    {
        [Header("References")]
        [SerializeField] private OnCollisionStayEvent _collisionStayEvent;

        #region MonoBehaviour

        private void OnValidate()
        {
            _collisionStayEvent ??= GetComponent<OnCollisionStayEvent>();
        }

        private void OnEnable()
        {
            _collisionStayEvent.onStay += Invoke;
        }

        private void OnDisable()
        {
            _collisionStayEvent.onStay -= Invoke;
        }

        #endregion

        private void Invoke(Collision collision) => Invoke();
    }
}
