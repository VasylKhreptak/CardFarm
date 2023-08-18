using CBA.Events.Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CBA.Events.UI.Input.Mono
{
    public class OnPointerMoveMonoEvent : MonoEvent
    {
        [Header("References")]
        [SerializeField] private OnPointerMoveEvent _onPointerMoveEvent;

        #region MonoBehaviour

        private void OnValidate()
        {
            _onPointerMoveEvent ??= GetComponent<OnPointerMoveEvent>();
        }

        private void OnEnable()
        {
            _onPointerMoveEvent.onPointerMove += Invoke;
        }

        private void OnDisable()
        {
            _onPointerMoveEvent.onPointerMove -= Invoke;
        }

        #endregion

        private void Invoke(BaseEventData data) => Invoke();
    }
}
