using UnityEngine;

namespace Extensions
{
    public static class ComponentExtensions
    {
        public static bool TryGetComponentInChildren<T>(this Component component, out T foundComponent, bool includeInactive = false) where T : Component
        {
            foundComponent = component.GetComponentInChildren<T>(includeInactive);
            return foundComponent != null;
        }
    }
}
