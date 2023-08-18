using CBA.Events.Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CBA.Events.UI.Input.Mono
{
    public class OnCancelMonoEvent : MonoEvent
    {
        [Header("References")]
        [SerializeField] private OnCancelEvent _onCancelEvent;

        #region MonoBehaviour

        private void OnValidate()
        {
            _onCancelEvent ??= GetComponent<OnCancelEvent>();
        }

        private void OnEnable()
        {
            _onCancelEvent.onCancel += Invoke;
        }

        private void OnDisable()
        {
            _onCancelEvent.onCancel -= Invoke;
        }

        #endregion

        private void Invoke(BaseEventData data) => Invoke();
    }
}
