using CBA.Events.Core;

namespace CBA.Events.Mono
{
    [UnityEngine.DisallowMultipleComponent]
    public class OnAwakeEvent : MonoEvent
    {
        #region MonoBehaviour

        private void Awake()
        {
            Invoke();
        }

        #endregion
    }
}
