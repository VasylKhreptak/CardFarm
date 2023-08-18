using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CBA.Events.UI.Input
{
    public class OnPointerUpEvent : MonoBehaviour, IPointerUpHandler
    {
        public event Action<PointerEventData> onUp;

        public void OnPointerUp(PointerEventData eventData)
        {
            onUp?.Invoke(eventData);
        }
    }
}
