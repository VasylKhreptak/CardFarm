using CBA.Events.Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CBA.Events.UI.Input.Mono
{
    public class OnPointerClickMonoEvent : MonoEvent
    {
        [Header("References")]
        [SerializeField] private OnPointerClickEvent _pointerClickEvent;

        #region MonoBehaviour

        private void OnValidate()
        {
            _pointerClickEvent ??= GetComponent<OnPointerClickEvent>();
        }

        private void OnEnable()
        {
            _pointerClickEvent.onClick += Invoke;
        }

        private void OnDisable()
        {
            _pointerClickEvent.onClick -= Invoke;
        }

        #endregion

        private void Invoke(PointerEventData data) => Invoke();
    }
}
