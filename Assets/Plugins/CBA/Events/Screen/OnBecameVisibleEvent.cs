using CBA.Events.Core;
using UnityEngine;

namespace CBA.Events.Screen
{
    [RequireComponent(typeof(Renderer))]
    public class OnBecameVisibleEvent : MonoEvent
    {
        private void OnBecameVisible()
        {
            Invoke();
        }
    }
}
