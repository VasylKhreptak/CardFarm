using CBA.Events.Core;

namespace CBA.Events.Mono
{
    [UnityEngine.DisallowMultipleComponent]
    public class OnLateUpdateEvent : MonoEvent
    {
        #region MonoBehaviour

        private void LateUpdate()
        {
            Invoke();
        }

        #endregion
    }
}
