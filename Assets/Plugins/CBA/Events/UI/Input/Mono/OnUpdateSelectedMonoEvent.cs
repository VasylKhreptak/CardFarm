using CBA.Events.Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CBA.Events.UI.Input.Mono
{
    public class OnUpdateSelectedMonoEvent : MonoEvent
    {
        [Header("References")]
        [SerializeField] private OnUpdateSelectedEvent _onUpdateSelectedEvent;

        #region MonoBehaviour

        private void OnValidate()
        {
            _onUpdateSelectedEvent ??= GetComponent<OnUpdateSelectedEvent>();
        }

        private void OnEnable()
        {
            _onUpdateSelectedEvent.onUpdateSelected += Invoke;
        }

        private void OnDisable()
        {
            _onUpdateSelectedEvent.onUpdateSelected -= Invoke;
        }

        #endregion

        private void Invoke(BaseEventData data) => Invoke();
    }
}
