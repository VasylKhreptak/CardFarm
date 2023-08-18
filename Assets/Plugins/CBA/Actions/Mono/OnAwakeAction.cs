using CBA.Actions.Mono.Core;

namespace CBA.Actions.Mono
{
    public class OnAwakeAction : ActionContainer
    {
        #region MonoBehaviour
        
        private void Awake()
        {
            _action.Do();
        }

        #endregion
    }
}
