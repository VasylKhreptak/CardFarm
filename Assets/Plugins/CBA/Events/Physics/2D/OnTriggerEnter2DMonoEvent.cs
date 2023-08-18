using CBA.Events.Core;
using UnityEngine;

namespace CBA.Events.Physics._2D
{
    public class OnTriggerEnter2DMonoEvent : MonoEvent
    {
        [Header("References")]
        [SerializeField] private OnTriggerEnter2DEvent _triggerEnterEvent;

        #region MonoBehaviour

        private void OnValidate()
        {
            _triggerEnterEvent ??= GetComponent<OnTriggerEnter2DEvent>();
        }

        private void OnEnable()
        {
            _triggerEnterEvent.onEnter += Invoke;
        }

        private void OnDisable()
        {
            _triggerEnterEvent.onEnter -= Invoke;
        }

        #endregion

        private void Invoke(Collider2D collider) => Invoke();
    }
}
