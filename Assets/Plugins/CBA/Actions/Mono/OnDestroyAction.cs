using CBA.Actions.Mono.Core;

namespace CBA.Actions.Mono
{
    public class OnDestroyAction : ActionContainer
    {
        #region MonoBehaviour
        
        private void OnDestroy()
        {
            _action.Do();
        }

        #endregion
    }
}
