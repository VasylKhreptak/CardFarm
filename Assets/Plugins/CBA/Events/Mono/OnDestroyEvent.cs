using CBA.Events.Core;

namespace CBA.Events.Mono
{
    [UnityEngine.DisallowMultipleComponent]
    public class OnDestroyEvent : MonoEvent
    {
        #region MonoBehaviour

        private void OnDestroy()
        {
            Invoke();
        }

        #endregion
    }
}
