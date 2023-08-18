using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CBA.Events.UI.Input
{
    public class OnSelectedEvent : MonoBehaviour, ISelectHandler
    {
        public event Action<BaseEventData> onSelected;

        public void OnSelect(BaseEventData eventData)
        {
            onSelected?.Invoke(eventData);
        }
    }
}
