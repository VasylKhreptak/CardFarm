using CBA.Events.Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CBA.Events.UI.Input.Mono
{
    public class OnPointerExitMonoEvent : MonoEvent
    {
        [Header("References")]
        [SerializeField] private OnPointerExitEvent _pointerExitEvent;

        #region MonoBehaviour

        private void OnValidate()
        {
            _pointerExitEvent ??= GetComponent<OnPointerExitEvent>();
        }

        private void OnEnable()
        {
            _pointerExitEvent.onExit += Invoke;
        }

        private void OnDisable()
        {
            _pointerExitEvent.onExit -= Invoke;
        }

        #endregion

        private void Invoke(PointerEventData data) => Invoke();
    }
}
