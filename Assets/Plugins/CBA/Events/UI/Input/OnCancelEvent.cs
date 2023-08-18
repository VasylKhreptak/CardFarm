using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CBA.Events.UI.Input
{
    public class OnCancelEvent : MonoBehaviour, ICancelHandler
    {
        public event Action<BaseEventData> onCancel;

        public void OnCancel(BaseEventData eventData)
        {
            onCancel?.Invoke(eventData);
        }
    }
}
