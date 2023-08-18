using CBA.Events.Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CBA.Events.UI.Input.Mono
{
    public class OnInitializePotentialDragMonoEvent : MonoEvent
    {
        [Header("References")]
        [SerializeField] private OnInitializePotentialDragEvent _onInitializePotentialDragEvent;

        #region MonoBehaviour

        private void OnValidate()
        {
            _onInitializePotentialDragEvent ??= GetComponent<OnInitializePotentialDragEvent>();
        }

        private void OnEnable()
        {
            _onInitializePotentialDragEvent.onInitializePotentialDrag += Invoke;
        }

        private void OnDisable()
        {
            _onInitializePotentialDragEvent.onInitializePotentialDrag -= Invoke;
        }

        #endregion

        private void Invoke(PointerEventData data) => Invoke();
    }
}
