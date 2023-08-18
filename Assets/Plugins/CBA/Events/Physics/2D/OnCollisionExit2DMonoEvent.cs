using CBA.Events.Core;
using UnityEngine;

namespace CBA.Events.Physics._2D
{
    public class OnCollisionExit2DMonoEvent : MonoEvent
    {
        [Header("References")]
        [SerializeField] private OnCollisionExit2DEvent _collisionExitEvent;

        #region MonoBehaviour

        private void OnValidate()
        {
            _collisionExitEvent ??= GetComponent<OnCollisionExit2DEvent>();
        }

        private void OnEnable()
        {
            _collisionExitEvent.onExit += Invoke;
        }

        private void OnDisable()
        {
            _collisionExitEvent.onExit -= Invoke;
        }

        #endregion

        private void Invoke(Collision2D collision) => Invoke();
    }
}
