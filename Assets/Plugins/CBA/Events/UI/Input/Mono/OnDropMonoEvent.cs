using CBA.Events.Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CBA.Events.UI.Input.Mono
{
    public class OnDropMonoEvent : MonoEvent
    {
        [Header("References")]
        [SerializeField] private OnDropEvent _dropEvent;

        #region MonoBehaviour

        private void OnValidate()
        {
            _dropEvent ??= GetComponent<OnDropEvent>();
        }

        private void OnEnable()
        {
            _dropEvent.onDrop += Invoke;
        }

        private void OnDisable()
        {
            _dropEvent.onDrop -= Invoke;
        }

        #endregion

        private void Invoke(PointerEventData data) => Invoke();
    }
}
