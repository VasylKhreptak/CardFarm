using CBA.Events.Core;

namespace CBA.Events.Mono
{
    [UnityEngine.DisallowMultipleComponent]
    public class OnFixedUpdateEvent : MonoEvent
    {
        #region MonoBehaviour

        private void FixedUpdate()
        {
            Invoke();
        }

        #endregion
    }
}
