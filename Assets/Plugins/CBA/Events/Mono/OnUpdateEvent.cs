using CBA.Events.Core;

namespace CBA.Events.Mono
{
    [UnityEngine.DisallowMultipleComponent]
    public class OnUpdateEvent : MonoEvent
    {
        #region MonoBehaviour

        private void Update()
        {
            Invoke();
        }

        #endregion
    }
}
