using CBA.Events.Core;

namespace CBA.Events.Mono
{
    [UnityEngine.DisallowMultipleComponent]
    public class OnEnableEvent : MonoEvent
    {
        #region MonoBehaviour

        private void OnEnable()
        {
            Invoke();
        }

        #endregion
    }
}
