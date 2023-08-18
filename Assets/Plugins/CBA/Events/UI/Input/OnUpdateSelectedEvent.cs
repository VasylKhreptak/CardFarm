using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CBA.Events.UI.Input
{
    public class OnUpdateSelectedEvent : MonoBehaviour, IUpdateSelectedHandler
    {
        public event Action<BaseEventData> onUpdateSelected;

        public void OnUpdateSelected(BaseEventData eventData)
        {
            onUpdateSelected?.Invoke(eventData);
        }
    }
}
