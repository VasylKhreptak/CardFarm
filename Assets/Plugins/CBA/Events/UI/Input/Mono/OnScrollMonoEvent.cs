using CBA.Events.Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CBA.Events.UI.Input.Mono
{
    public class OnScrollMonoEvent : MonoEvent
    {
        [Header("References")]
        [SerializeField] private OnScrollEvent _onScrollEvent;

        #region MonoBehaviour

        private void OnValidate()
        {
            _onScrollEvent ??= GetComponent<OnScrollEvent>();
        }

        private void OnEnable()
        {
            _onScrollEvent.onScroll += Invoke;
        }

        private void OnDisable()
        {
            _onScrollEvent.onScroll -= Invoke;
        }

        #endregion

        private void Invoke(PointerEventData data) => Invoke();
    }
}
