using CBA.Actions.Mono.Core;

namespace CBA.Actions.Mono
{
    public class OnUpdateAction : ActionContainer
    {
        #region MonoBehaviour
        
        private void Update()
        {
            _action.Do();
        }

        #endregion
    }
}
