using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CBA.Events.UI.Input
{
    public class OnBeginDragEvent : MonoBehaviour, IBeginDragHandler
    {
        public event Action<BaseEventData> onBeginDrag;

        public void OnBeginDrag(PointerEventData eventData)
        {
            onBeginDrag?.Invoke(eventData);
        }
    }
}
