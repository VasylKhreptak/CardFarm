using CBA.Events.Core;
using UnityEngine;

namespace CBA.Events.Physics._2D
{
    public class OnCollisionStay2DMonoEvent : MonoEvent
    {
        [Header("References")]
        [SerializeField] private OnCollisionStay2DEvent _collisionStayEvent;

        #region MonoBehaviour

        private void OnValidate()
        {
            _collisionStayEvent ??= GetComponent<OnCollisionStay2DEvent>();
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

        private void Invoke(Collision2D collision) => Invoke();
    }
}
