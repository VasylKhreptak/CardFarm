using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CBA.Events.UI.Input
{
    public class OnMoveEvent : MonoBehaviour, IMoveHandler
    {
        public event Action<AxisEventData> onMove;

        public void OnMove(AxisEventData eventData)
        {
            onMove?.Invoke(eventData);
        }
    }
}
