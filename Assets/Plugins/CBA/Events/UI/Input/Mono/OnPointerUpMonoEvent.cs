using CBA.Events.Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CBA.Events.UI.Input.Mono
{
    public class OnPointerUpMonoEvent : MonoEvent
    {
        [Header("References")]
        [SerializeField] private OnPointerUpEvent _pointerUpEvent;

        #region MonoBehaviour

        private void OnValidate()
        {
            _pointerUpEvent ??= GetComponent<OnPointerUpEvent>();
        }

        private void OnEnable()
        {
            _pointerUpEvent.onUp += Invoke;
        }

        private void OnDisable()
        {
            _pointerUpEvent.onUp -= Invoke;
        }

        #endregion

        private void Invoke(PointerEventData data) => Invoke();
    }
}
