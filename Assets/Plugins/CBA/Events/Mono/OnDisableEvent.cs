using CBA.Events.Core;

namespace CBA.Events.Mono
{
    [UnityEngine.DisallowMultipleComponent]
    public class OnDisableEvent : MonoEvent
    {
        #region MonoBehaviour

        private void OnDisable()
        {
            Invoke();
        }

        #endregion
    }
}
