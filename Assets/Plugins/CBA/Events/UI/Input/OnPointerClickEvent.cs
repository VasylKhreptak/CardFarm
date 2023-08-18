using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CBA.Events.UI.Input
{
    public class OnPointerClickEvent : MonoBehaviour, IPointerClickHandler
    {
        public event Action<PointerEventData> onClick;

        public void OnPointerClick(PointerEventData eventData)
        {
            onClick?.Invoke(eventData);
        }
    }
}
