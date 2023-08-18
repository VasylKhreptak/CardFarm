using CBA.Events.Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CBA.Events.UI.Input.Mono
{
    public class OnDragMonoEvent : MonoEvent
    {
        [Header("References")]
        [SerializeField] private OnDragEvent _dragEvent;

        #region MonoBehaviour

        private void OnValidate()
        {
            _dragEvent ??= GetComponent<OnDragEvent>();
        }

        private void OnEnable()
        {
            _dragEvent.onDrag += Invoke;
        }

        private void OnDisable()
        {
            _dragEvent.onDrag -= Invoke;
        }

        #endregion

        private void Invoke(PointerEventData data) => Invoke();
    }
}
