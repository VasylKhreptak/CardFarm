using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CBA.Events.UI.Input
{
    public class OnPointerMoveEvent : MonoBehaviour, IPointerMoveHandler
    {
        public event Action<BaseEventData> onPointerMove;

        public void OnPointerMove(PointerEventData eventData)
        {
            onPointerMove?.Invoke(eventData);
        }
    }
}
