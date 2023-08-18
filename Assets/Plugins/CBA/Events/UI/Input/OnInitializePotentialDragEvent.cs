using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CBA.Events.UI.Input
{
    public class OnInitializePotentialDragEvent : MonoBehaviour, IInitializePotentialDragHandler
    {
        public event Action<PointerEventData> onInitializePotentialDrag;

        public void OnInitializePotentialDrag(PointerEventData eventData)
        {
            onInitializePotentialDrag?.Invoke(eventData);
        }
    }
}
