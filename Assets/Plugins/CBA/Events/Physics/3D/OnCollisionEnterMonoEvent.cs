using CBA.Events.Core;
using UnityEngine;

namespace CBA.Events.Physics._3D
{
    public class OnCollisionEnterMonoEvent : MonoEvent
    {
        [Header("References")]
        [SerializeField] private OnCollisionEnterEvent _collisionEnterEvent;

        #region MonoBehaviour

        private void OnValidate()
        {
            _collisionEnterEvent ??= GetComponent<OnCollisionEnterEvent>();
        }

        private void OnEnable()
        {
            _collisionEnterEvent.onEnter += Invoke;
        }

        private void OnDisable()
        {
            _collisionEnterEvent.onEnter -= Invoke;
        }

        #endregion

        private void Invoke(Collision collision) => Invoke();
    }
}
