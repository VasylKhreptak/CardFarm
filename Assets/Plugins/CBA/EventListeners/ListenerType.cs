using UnityEngine;

namespace CBA.EventListeners
{
    public enum ListenerType
    {
        [InspectorName("Awake, OnDestroy")] AwakeDestroy = 0,
        [InspectorName("OnEnable, OnDisable")] EnableDisable = 1,
        [InspectorName("Start, OnDestroy")] StartDestroy = 2,
        [InspectorName("Fully manual")] FullyManual = 3,
        [InspectorName("Editor only")] Editor = 4,
    }
}
