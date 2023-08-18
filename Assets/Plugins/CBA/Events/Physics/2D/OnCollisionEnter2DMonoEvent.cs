using CBA.Events.Core;
using UnityEngine;

namespace CBA.Events.Physics._2D
{
    public class OnCollisionEnter2DMonoEvent : MonoEvent
    {
        [Header("References")]
        [SerializeField] private OnCollisionEnter2DEvent _collisionEnterEvent;

        #region MonoBehaviour

        private void OnValidate()
        {
            _collisionEnterEvent ??= GetComponent<OnCollisionEnter2DEvent>();
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

        private void Invoke(Collision2D collision) => Invoke();
    }
}
