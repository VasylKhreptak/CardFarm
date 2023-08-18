using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CBA.Events.UI.Input
{
    public class OnScrollEvent : MonoBehaviour, IScrollHandler
    {
        public event Action<PointerEventData> onScroll;

        public void OnScroll(PointerEventData eventData)
        {
            onScroll?.Invoke(eventData);
        }
    }
}
