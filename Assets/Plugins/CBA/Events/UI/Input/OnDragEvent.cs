using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CBA.Events.UI.Input
{
    public class OnDragEvent : MonoBehaviour, IDragHandler
    {
        public event Action<PointerEventData> onDrag;

        public void OnDrag(PointerEventData eventData)
        {
            onDrag?.Invoke(eventData);
        }
    }
}
