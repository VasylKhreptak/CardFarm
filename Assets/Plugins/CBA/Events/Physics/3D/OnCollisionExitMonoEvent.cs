using CBA.Events.Core;
using UnityEngine;

namespace CBA.Events.Physics._3D
{
    public class OnCollisionExitMonoEvent : MonoEvent
    {
        [Header("References")]
        [SerializeField] private OnCollisionExitEvent _collisionExitEvent;

        #region MonoBehaviour

        private void OnValidate()
        {
            _collisionExitEvent ??= GetComponent<OnCollisionExitEvent>();
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

        private void Invoke(Collision collision) => Invoke();
    }
}
