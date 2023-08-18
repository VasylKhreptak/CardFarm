using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CBA.Events.UI.Input
{
    public class OnPointerDownEvent : MonoBehaviour, IPointerDownHandler
    {
        public event Action<PointerEventData> onDown;

        public void OnPointerDown(PointerEventData eventData)
        {
            onDown?.Invoke(eventData);
        }
    }
}
