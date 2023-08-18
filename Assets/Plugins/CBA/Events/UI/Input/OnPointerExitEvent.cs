using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CBA.Events.UI.Input
{
    public class OnPointerExitEvent : MonoBehaviour, IPointerExitHandler
    {
        public event Action<PointerEventData> onExit;

        public void OnPointerExit(PointerEventData eventData)
        {
            onExit?.Invoke(eventData);
        }
    }
}
