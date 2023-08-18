using CBA.Events.Core;
using UnityEngine;

namespace CBA.Events.Screen
{
    [RequireComponent(typeof(Renderer))]
    public class OnBecameInvisibleEvent : MonoEvent
    {
        private void OnBecameInvisible()
        {
            Invoke();
        }
    }
}
