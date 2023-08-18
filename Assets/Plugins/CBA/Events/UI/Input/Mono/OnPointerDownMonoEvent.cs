using CBA.Events.Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CBA.Events.UI.Input.Mono
{
    public class OnPointerDownMonoEvent : MonoEvent
    {
        [Header("References")]
        [SerializeField] private OnPointerDownEvent _pointerDownEvent;

        #region MonoBehaviour

        private void OnValidate()
        {
            _pointerDownEvent ??= GetComponent<OnPointerDownEvent>();
        }

        private void OnEnable()
        {
            _pointerDownEvent.onDown += Invoke;
        }

        private void OnDisable()
        {
            _pointerDownEvent.onDown -= Invoke;
        }

        #endregion

        private void Invoke(PointerEventData data) => Invoke();
    }
}
