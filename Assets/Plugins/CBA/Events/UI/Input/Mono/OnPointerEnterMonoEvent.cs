using CBA.Events.Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CBA.Events.UI.Input.Mono
{
    public class OnPointerEnterMonoEvent : MonoEvent
    {
        [Header("References")]
        [SerializeField] private OnPointerEnterEvent _pointerEnterEvent;

        #region MonoBehaviour

        private void OnValidate()
        {
            _pointerEnterEvent ??= GetComponent<OnPointerEnterEvent>();
        }

        private void OnEnable()
        {
            _pointerEnterEvent.onEnter += Invoke;
        }

        private void OnDisable()
        {
            _pointerEnterEvent.onEnter -= Invoke;
        }

        #endregion

        private void Invoke(PointerEventData data) => Invoke();
    }
}
