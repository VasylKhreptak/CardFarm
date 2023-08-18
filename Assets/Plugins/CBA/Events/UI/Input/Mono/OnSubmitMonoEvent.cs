using CBA.Events.Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CBA.Events.UI.Input.Mono
{
    public class OnSubmitMonoEvent : MonoEvent
    {
        [Header("References")]
        [SerializeField] private OnSubmitEvent _onSubmitEvent;

        #region MonoBehaviour

        private void OnValidate()
        {
            _onSubmitEvent ??= GetComponent<OnSubmitEvent>();
        }

        private void OnEnable()
        {
            _onSubmitEvent.onSubmit += Invoke;
        }

        private void OnDisable()
        {
            _onSubmitEvent.onSubmit -= Invoke;
        }

        #endregion

        private void Invoke(BaseEventData data) => Invoke();
    }
}
