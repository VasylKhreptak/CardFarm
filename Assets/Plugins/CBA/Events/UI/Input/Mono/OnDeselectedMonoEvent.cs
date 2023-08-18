using CBA.Events.Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CBA.Events.UI.Input.Mono
{
    public class OnDeselectedMonoEvent : MonoEvent
    {
        [Header("References")]
        [SerializeField] private OnDeselectedEvent _onDeselectedEvent;

        #region MonoBehaviour

        private void OnValidate()
        {
            _onDeselectedEvent ??= GetComponent<OnDeselectedEvent>();
        }

        private void OnEnable()
        {
            _onDeselectedEvent.onDeselected += Invoke;
        }

        private void OnDisable()
        {
            _onDeselectedEvent.onDeselected -= Invoke;
        }

        #endregion

        private void Invoke(BaseEventData data) => Invoke();
    }
}
