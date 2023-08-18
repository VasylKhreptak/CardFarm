using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CBA.Events.UI.Input
{
    public class OnDeselectedEvent : MonoBehaviour, ISelectHandler
    {
        public event Action<BaseEventData> onDeselected;

        public void OnSelect(BaseEventData eventData)
        {
            onDeselected?.Invoke(eventData);
        }
    }
}
