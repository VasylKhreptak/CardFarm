using CBA.Actions.Mono.Core;

namespace CBA.Actions.Mono
{
    public class OnDisableAction : ActionContainer
    {
        #region MonoBehaviour

        private void OnDisable()
        {
            _action.Do();
        }

        #endregion
    }
}
