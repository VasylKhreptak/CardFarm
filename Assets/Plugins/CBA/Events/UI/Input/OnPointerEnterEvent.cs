using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CBA.Events.UI.Input
{
    public class OnPointerEnterEvent : MonoBehaviour, IPointerEnterHandler
    {
        public event Action<PointerEventData> onEnter;

        public void OnPointerEnter(PointerEventData eventData)
        {
            onEnter?.Invoke(eventData);
        }
    }
}
