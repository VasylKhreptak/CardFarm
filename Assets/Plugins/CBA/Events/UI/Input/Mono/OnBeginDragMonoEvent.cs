using CBA.Events.Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CBA.Events.UI.Input.Mono
{
    public class OnBeginDragMonoEvent : MonoEvent
    {
        [Header("References")]
        [SerializeField] private OnBeginDragEvent _onBeginDragEvent;

        #region MonoBehaviour

        private void OnValidate()
        {
            _onBeginDragEvent ??= GetComponent<OnBeginDragEvent>();
        }

        private void OnEnable()
        {
            _onBeginDragEvent.onBeginDrag += Invoke;
        }

        private void OnDisable()
        {
            _onBeginDragEvent.onBeginDrag -= Invoke;
        }

        #endregion

        private void Invoke(BaseEventData data) => Invoke();
    }
}
