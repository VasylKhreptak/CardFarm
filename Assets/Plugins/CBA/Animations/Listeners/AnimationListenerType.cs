using UnityEngine;

namespace CBA.Animations.Listeners
{
    public enum  AnimationListenerType
    {
        [InspectorName("Awake, OnDestroy")] AwakeOnDestroy = 0,
        [InspectorName("Start, OnDestroy")] StartOnDestroy = 1,
        [InspectorName("OnEnable, OnDisable")] OnEnableOnDisable = 2,
    }
}
