using CBA.Actions.Mono.Core;

namespace CBA.Actions.Mono
{
    public class OnFixedUpdateAction : ActionContainer
    {
        #region MonoBehaviour
        
        private void FixedUpdate()
        {
            _action.Do();
        }

        #endregion
    }
}
