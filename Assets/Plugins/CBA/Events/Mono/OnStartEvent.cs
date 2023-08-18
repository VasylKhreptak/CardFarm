using CBA.Events.Core;

namespace CBA.Events.Mono
{
    [UnityEngine.DisallowMultipleComponent]
    public class OnStartEvent : MonoEvent
    {
        #region MonoBehaviour

        private void Start()
        {
            Invoke();
        }

        #endregion
    }
}
