using CBA.Events.Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CBA.Events.UI.Input.Mono
{
    public class OnSelectedMonoEvent : MonoEvent
    {
        [Header("References")]
        [SerializeField] private OnSelectedEvent _onSelectedEvent;

        #region MonoBehaviour

        private void OnValidate()
        {
            _onSelectedEvent ??= GetComponent<OnSelectedEvent>();
        }

        private void OnEnable()
        {
            _onSelectedEvent.onSelected += Invoke;
        }

        private void OnDisable()
        {
            _onSelectedEvent.onSelected -= Invoke;
        }

        #endregion

        private void Invoke(BaseEventData data) => Invoke();
    }
}
