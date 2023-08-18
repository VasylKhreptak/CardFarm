using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CBA.Events.UI.Input
{
    public class OnSubmitEvent : MonoBehaviour, ISubmitHandler
    {
        public event Action<BaseEventData> onSubmit;

        public void OnSubmit(BaseEventData eventData)
        {
            onSubmit?.Invoke(eventData);
        }
    }
}
