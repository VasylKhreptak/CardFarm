using CBA.Actions.Mono.Core;

namespace CBA.Actions.Mono
{
    public class OnLateUpdateAction : ActionContainer
    {
        #region MonoBehaviour

        private void LateUpdate()
        {
            _action.Do();
        }

        #endregion
    }
}
