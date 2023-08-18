using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CBA.Events.UI.Input
{
    public class OnDropEvent : MonoBehaviour, IDropHandler
    {
        public event Action<PointerEventData> onDrop;

        public void OnDrop(PointerEventData eventData)
        {
            onDrop?.Invoke(eventData);
        }
    }
}
