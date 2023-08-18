using CBA.Events.Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CBA.Events.UI.Input.Mono
{
    public class OnMoveMonoEvent : MonoEvent
    {
        [Header("References")]
        [SerializeField] private OnMoveEvent _onMoveEvent;

        #region MonoBehaviour

        private void OnValidate()
        {
            _onMoveEvent ??= GetComponent<OnMoveEvent>();
        }

        private void OnEnable()
        {
            _onMoveEvent.onMove += Invoke;
        }

        private void OnDisable()
        {
            _onMoveEvent.onMove -= Invoke;
        }

        #endregion

        private void Invoke(AxisEventData data) => Invoke();
    }
}
